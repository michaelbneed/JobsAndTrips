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

			lblCompanyTitle.Text = nextJob.PropertyName ?? "";

			lblPropertyAddress.Text = (nextJob.Street1 ?? "") + " " + (nextJob.Street2 ?? "") + "n\n" +
				" " + (nextJob.City ?? "" ) + ", " + (nextJob.State ?? "") + " " + (nextJob.ZipCode ?? "");

			lblPropertyPhone.Text = nextJob.SalesRepPhone ?? "";

			lblServiceName.Text = nextJob.JobServiceName ?? "";

			lblAccountManager.Text = nextJob.SalesRepContactName ?? "";

			lblPropertyName.Text = nextJob.PropertyName ?? "";

			// TODO: figure out what should be used if there is no sitefotos url
			webView.Source = nextJob.SiteFotosUrl ?? "https://www.sitefotos.com/vpics/guestmapdev?y3v7h0";
		}

		public void OnClockInOutClicked(object sender, EventArgs e)
		{
			if ((sender as Button).Text.Contains("Clock In"))
			{
				ClockInDto clockInDto = new ClockInDto();
				clockInDto.JobId = long.Parse((sender as Button).CommandParameter.ToString());
				clockInDto.ActualClockIn = DateTime.Now;
				clockInDto.ActualWeightIn = null;  // TODO: need to get weigh in from UI
				TruckDto truckRegistration = CredentialsManager.GetLoginCredentials();
				ApiClient.ApiClient client = new ApiClient.ApiClient(truckRegistration.TruckNumber, truckRegistration.ApiKey);
				if (client.ClockIn(clockInDto))
				{
					(sender as Button).Text = "Clock Out \n (" + DateTime.Now + ")"; ;
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
		}

		public void ButtonConfirmServiceClicked(object sender, EventArgs e)
		{
			
			lblServiceConfirmMsg.Text = "Demo Complete";
			stkWebViewSiteMap.IsVisible = false;
			lblServiceConfirmMsg.TextColor = Color.DarkGreen;
			lblServiceCompleteMsg.IsVisible = true;
			lblServiceCompleteMsg.TextColor = Color.DarkGreen;
			stkBtnConfirm.IsVisible = false;

			stkServiceChoices.IsVisible = false;
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

			lblServiceConfirmMsg.Text = "Demo Complete";
			stkWebViewSiteMap.IsVisible = false;
			lblServiceConfirmMsg.TextColor = Color.DarkGreen;
			lblServiceCompleteMsg.IsVisible = true;
			lblServiceCompleteMsg.TextColor = Color.DarkGreen;
		}

		private void BtnCall_Click(object sender, EventArgs e)
		{
			var call = CrossMessaging.Current.PhoneDialer;
			if (call.CanMakePhoneCall)
			{
				call.MakePhoneCall("317-123-1234");
			}
		}
	}
}