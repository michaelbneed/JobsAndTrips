using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trip.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Messaging;
//using Trip.ApiClient;
using Trip.BusinessObjects.Models;

namespace Trip.Views
{
	[DesignTimeVisible(true)]
	public partial class SiteMapPage : ContentPage
	{
		public JobDto CurrentJob
		{ get { return TripContext.CurrentJob; } set { TripContext.CurrentJob = value; } }

		public TruckDto Credentials
		{ get { return TripContext.Credentials; } set { TripContext.Credentials = value; } }

		public SiteMapPage()
		{
			InitializeComponent();
			TripContext.CurrentPage = "SiteMapPage";
			BindPageFields();
		}

		private void BindPageFields()
		{
			btnClockInClick.Text = "Clock In \n (" + DateTime.Now + ")";

			btnClockInClick.CommandParameter = CurrentJob.JobId.ToString();

			lblOpsContactName.Text = "Sales: " + (CurrentJob.OperationsContactName ?? "");
			lblOpsContactPhone.Text = CurrentJob.OperationsContactPhone ?? "";

			lblPropertyName.Text = CurrentJob.PropertyName ?? "";
			lblPropertyAddress.Text = (CurrentJob.Street1 ?? "") + " " + (CurrentJob.Street2 ?? "") + "\n" +
				" " + (CurrentJob.City ?? "") + ", " + (CurrentJob.State ?? "") + " " + (CurrentJob.ZipCode ?? "");

			lblServiceName.Text = CurrentJob.JobServiceName ?? "";

			lblAccountManagerName.Text = "Operations: " + (CurrentJob.SalesRepContactName ?? "");
			lblAccountManagerPhone.Text = CurrentJob.SalesRepPhone ?? "";

			// TODO: figure out what should be used if there is no sitefotos url
			webView.Source = CurrentJob.SiteFotosUrl ?? "https://www.sitefotos.com/vpics/guestmapdev?y3v7h0";

			stkWeighOut.IsVisible = CurrentJob.JobRequiresWeighInOut;
			stkWeighIn.IsVisible = CurrentJob.JobRequiresWeighInOut;

			// TODO: Resume current state: Clock-In, Clock-Out, Clock-Out-Choices
			if (TripContext.ClockInOutState == "ClockIn")
			{
				SetClockInDisplay(btnClockInClick);
			}
			if (TripContext.ClockInOutState == "ClockOut")
			{
				SetClockOutDisplay(btnClockInClick);
			}
			else if (TripContext.ClockInOutState == "SiteComplete")
			{
				SetSiteCompleteDisplay(btnClockInClick);
			}
		}

		private void SetClockInDisplay(object sender)
		{
			(sender as Button).Text = "Clock In";
			(sender as Button).IsVisible = true;
			grdCurrentSite.IsVisible = true;
			lblSpcInstructions.IsVisible = true;
			webView.IsVisible = true;
			stkWeighIn.IsVisible = CurrentJob.JobRequiresWeighInOut;
			TripContext.ClockInOutState = "ClockIn";
		}
		private void SetClockOutDisplay(object sender)
		{
			(sender as Button).Text = "Clock Out \n (" + DateTime.Now + ")";
			(sender as Button).BackgroundColor = Color.Red;
			stkWeighIn.IsVisible = false;
			stkWeighOut.IsVisible = CurrentJob.JobRequiresWeighInOut;
			TripContext.ClockInOutState = "ClockOut";
		}

		private void SetSiteCompleteDisplay(object sender)
		{
			grdCurrentSite.IsVisible = false;
			lblSpcInstructions.IsVisible = false;
			webView.IsVisible = false;
			(sender as Button).IsVisible = false;
			stkWeighIn.IsVisible = false;
			stkWeighOut.IsVisible = CurrentJob.JobRequiresWeighInOut;
			grdSiteComplete.IsVisible = true;

			TripContext.ClockInOutState = "SiteComplete";
		}

		private void SetServiceChoicesDisplay(object sender)
		{
			stkServiceChoices.IsVisible = true;
			stkBtnConfirm.IsVisible = false;
		}

		public async void OnClockInOutClicked(object sender, EventArgs e)
		{
			if ((sender as Button).Text.Contains("Clock In"))
			{
				ClockInDto clockInDto = new ClockInDto();
				clockInDto.JobId = CurrentJob.JobId; // long.Parse((sender as Button).CommandParameter.ToString());
				clockInDto.ActualClockIn = DateTime.Now;
				
				if (CurrentJob.JobRequiresWeighInOut)
				{
					double weighInValue = 0.0;
					if (double.TryParse(weighInEntry.Text, out weighInValue))
						clockInDto.ActualWeightIn = weighInValue;
					else
					{
						await DisplayAlert("Weigh In is Required", "Please Proceed directly to the scales and Weigh In before doing any work on the site.", "OK");
						return;
					}
				}
				bool clockedIn = await TripContext.ClockIn(clockInDto);
				if (clockedIn)
				{
					SetClockOutDisplay(sender);
				}
				else
				{
					(sender as Button).Text = "Clock In failed. Please re-try. \n (" + DateTime.Now + ")";
				}
			}
			else
			{
				SetSiteCompleteDisplay(sender);
			}
		}

		public async void ButtonConfirmServiceClicked(object sender, EventArgs e)
		{
			bool success = await DoClockOut(lblServiceName.Text);
			if (success)
			{
				lblServiceConfirmMsg.Text = "Clock-Out Completed. Checking for the next Job...";
				stkWebViewSiteMap.IsVisible = false;
				lblServiceConfirmMsg.TextColor = Color.DarkGreen;
				lblServiceCompleteMsg.IsVisible = true;
				lblServiceCompleteMsg.TextColor = Color.DarkGreen;
				stkBtnConfirm.IsVisible = false;
				stkServiceChoices.IsVisible = false;
				success = await TripContext.GetNextJob();
				await Navigation.PopToRootAsync();
				await Navigation.PushAsync(new DirectionsPage());
			}
			else
			{
				lblServiceConfirmMsg.Text = "Clock-Out Encountered an Error. Please try again.";
				lblServiceConfirmMsg.TextColor = Color.DarkRed;
				stkBtnConfirm.IsVisible = true;
			}
		}

		private async Task<bool> DoClockOut(string actualServicePerformed)
		{
			ClockOutDto clockOutDto = new ClockOutDto();
			clockOutDto.JobId = long.Parse(this.btnClockInClick.CommandParameter.ToString());
			clockOutDto.ActualClockOut = DateTime.Now;
			clockOutDto.ActualServicePerformed = actualServicePerformed;
			double weight = 0.0;

			if (CurrentJob.JobRequiresWeighInOut && !actualServicePerformed.StartsWith("Skipped"))
			{
				if (double.TryParse(txtWeightOut.Text, out weight))
					clockOutDto.ActualWeightOut = weight;
				else
				{
					await DisplayAlert("Weigh Out is Required", "Before you clock out and leave the site, you must Weigh Out.", "OK");
					SetSiteCompleteDisplay(btnClockInClick);
					return false;
				}
			}
			bool success = await TripContext.ClockOut(clockOutDto);
			return success;
		}

		public void ButtonDenyServiceClicked(object sender, EventArgs e)
		{
			SetServiceChoicesDisplay(sender);
		}

		public async void ButtonCompleteUnfinishedJobClick(object sender, EventArgs e)
		{
			lblServiceCompleteMsg.IsVisible = true;
			
			stkServiceChoices.IsVisible = false;
			stkBtnConfirm.IsVisible = false;

			lblServiceConfirmMsg.Text = "Clock-Out Completed. Posting Clock-Out and Checking for the next Job...";
			stkWebViewSiteMap.IsVisible = false;
			lblServiceConfirmMsg.TextColor = Color.DarkGreen;
			lblServiceCompleteMsg.IsVisible = true;
			lblServiceCompleteMsg.TextColor = Color.DarkGreen;

			var actualServiceName = (sender as Button).Text;
			bool success = await DoClockOut(actualServiceName);
		}

		private void BtnCallOps_Click(object sender, EventArgs e)
		{
			var call = CrossMessaging.Current.PhoneDialer;
			if (call.CanMakePhoneCall && !string.IsNullOrEmpty(lblOpsContactPhone.Text))
			{
				call.MakePhoneCall(lblOpsContactPhone.Text);
			}
		}

		private void BtnCallSales_Click(object sender, EventArgs e)
		{
			var call = CrossMessaging.Current.PhoneDialer;
			if (call.CanMakePhoneCall && !string.IsNullOrEmpty(lblAccountManagerPhone.Text))
			{
				call.MakePhoneCall(lblAccountManagerPhone.Text);
			}
		}
	}
}