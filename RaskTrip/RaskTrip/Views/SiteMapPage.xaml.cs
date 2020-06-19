using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RaskTrip.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Messaging;
using RaskTrip.ApiClient;
using RaskTrip.BusinessObjects.Models;

namespace RaskTrip.Views
{
	[DesignTimeVisible(true)]
	public partial class SiteMapPage : ContentPage
	{
		static bool requireWeighIn = false;

		public SiteMapPage()
		{
			InitializeComponent();
			btnClockInClick.Text = "Clock In \n (" + DateTime.Now + ")";

			// TODO: Get data from storage
			JobDto nextJob = new JobDto();
			TruckDto truckRegistration = CredentialsManager.GetLoginCredentials();
			ApiClient.ApiClient client = new ApiClient.ApiClient(truckRegistration.TruckNumber, truckRegistration.ApiKey);

			nextJob = client.GetNextJob(truckRegistration.TruckId);

			btnClockInClick.CommandParameter = nextJob.JobId.ToString();

			lblOpsContactName.Text = "Sales: " + (nextJob.OperationsContactName ?? "");
			lblOpsContactPhone.Text = nextJob.OperationsContactPhone ?? "";

			lblPropertyName.Text = nextJob.PropertyName ?? "";
			lblPropertyAddress.Text = (nextJob.Street1 ?? "") + " " + (nextJob.Street2 ?? "") + "\n" +
				" " + (nextJob.City ?? "" ) + ", " + (nextJob.State ?? "") + " " + (nextJob.ZipCode ?? "");

			lblServiceName.Text = nextJob.JobServiceName ?? "";

			lblAccountManagerName.Text = "Operations: " + (nextJob.SalesRepContactName ?? "");
			lblAccountManagerPhone.Text = nextJob.SalesRepPhone ?? "";

			// TODO: figure out what should be used if there is no sitefotos url
			webView.Source = nextJob.SiteFotosUrl ?? "https://www.sitefotos.com/vpics/guestmapdev?y3v7h0";

			requireWeighIn = nextJob.JobRequiresWeighInOut;

			stkWeighOut.IsVisible = nextJob.JobRequiresWeighInOut;
		}

		public void OnClockInOutClicked(object sender, EventArgs e)
		{
			if ((sender as Button).Text.Contains("Clock In"))
			{
				ClockInDto clockInDto = new ClockInDto();
				clockInDto.JobId = long.Parse((sender as Button).CommandParameter.ToString());
				clockInDto.ActualClockIn = DateTime.Now;

				if (requireWeighIn)
				{
					weighInEntry.IsVisible = true;
				}

				clockInDto.ActualWeightIn = null;  // TODO: need to get weigh in from UI
				TruckDto truckRegistration = CredentialsManager.GetLoginCredentials();
				ApiClient.ApiClient client = new ApiClient.ApiClient(truckRegistration.TruckNumber, truckRegistration.ApiKey);
				if (client.ClockIn(clockInDto))
				{
					(sender as Button).Text = "Clock Out \n (" + DateTime.Now + ")";
					(sender as Button).BackgroundColor = Color.Red;
				}
				else
				{
					(sender as Button).Text = "Clock In failed. Please re-try. \n (" + DateTime.Now + ")";
				}
			}
			else
			{
				(sender as Button).IsVisible = false;
				grdSiteComplete.IsVisible = true;
				grdCurrentSite.IsVisible = false;
				lblSpcInstructions.IsVisible = false;
				webView.IsVisible = false;
			}

			weighInEntry.IsVisible = false;
		}

		public void ButtonConfirmServiceClicked(object sender, EventArgs e)
		{

			lblServiceConfirmMsg.Text = "Clock-Out Completed. Posting Clock-Out and Checking for the next Job...";
			stkWebViewSiteMap.IsVisible = false;
			lblServiceConfirmMsg.TextColor = Color.DarkGreen;
			lblServiceCompleteMsg.IsVisible = true;
			lblServiceCompleteMsg.TextColor = Color.DarkGreen;
			stkBtnConfirm.IsVisible = false;
			stkServiceChoices.IsVisible = false;

			bool success = DoClockOut(lblServiceName.Text);
			if (success)
			{
				Navigation.PopToRootAsync();
				Navigation.PushAsync(new DirectionsPage());
			}
			else
			{
				lblServiceConfirmMsg.Text = "Clock-Out Encountered an Error. Please try again.";
				lblServiceConfirmMsg.TextColor = Color.DarkRed;
				stkBtnConfirm.IsVisible = true;
			}
		}

		private bool DoClockOut(string actualServicePerformed)
		{
			ClockOutDto clockOutDto = new ClockOutDto();
			clockOutDto.JobId = long.Parse(this.btnClockInClick.CommandParameter.ToString());
			clockOutDto.ActualClockOut = DateTime.Now;
			clockOutDto.ActualServicePerformed = actualServicePerformed;
			double weight = 0.0;

			if (requireWeighIn)
			{
				if (double.TryParse(txtWeightOut.Text, out weight))
					clockOutDto.ActualWeightOut = weight;
			}
			
			var truckCredentials = CredentialsManager.GetLoginCredentials();
			var client = new ApiClient.ApiClient(truckCredentials.TruckNumber, truckCredentials.ApiKey);
			bool success = client.ClockOut(clockOutDto);
			return success;
		}

		public void ButtonDenyServiceClicked(object sender, EventArgs e)
		{
			stkServiceChoices.IsVisible = true;
			stkBtnConfirm.IsVisible = false;
		}

		public void ButtonCompleteUnfinishedJobClick(object sender, EventArgs e)
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
			bool success = DoClockOut(actualServiceName);
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