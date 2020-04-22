using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;
using Color = Xamarin.Forms.Color;
using Xamarin.Essentials;
using System.Diagnostics;
using Plugin.Messaging;
using RaskTrip.BusinessObjects;

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