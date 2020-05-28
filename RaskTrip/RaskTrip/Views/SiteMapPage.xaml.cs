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
			webView.Source = $"https://www.sitefotos.com/vpics/guestmapdev?y3v7h0";
			btnClockInClick.Text = "Clock In \n (" + DateTime.Now + ")";

			// TODO: Get data from storage
			JobDto nextJob = new JobDto();
			TruckDto truckRegistration = new TruckDto();
			truckRegistration.TruckId = 1;
			ApiClient.ApiClient client = new ApiClient.ApiClient();

			nextJob = client.GetNextJob(truckRegistration);

			lblCompanyTitle.Text = nextJob.PropertyName.ToString();

			lblPropertyAddress.Text = nextJob.Street1.ToString() + " " + nextJob.Street2.ToString() + "n\n" +
				" " + nextJob.City.ToString() + ", " + nextJob.State.ToString() + " " + nextJob.ZipCode.ToString();

			lblPropertyPhone.Text = nextJob.SalesRepPhone.ToString();

			lblServiceName.Text = nextJob.JobServiceName.ToString();

			lblAccountManager.Text = nextJob.SalesRepContactName.ToString();

			lblPropertyName.Text = nextJob.PropertyName.ToString();
		}

		public void OnClockInOutClicked(object sender, EventArgs e)
		{
			if ((sender as Button).Text.Contains("Clock In"))
			{
				(sender as Button).Text = "Clock Out \n (" + DateTime.Now + ")"; ;
				(sender as Button).BackgroundColor = Color.Red;
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