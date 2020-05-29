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
	public partial class DirectionsPage : ContentPage
	{
		Geocoder geoCoder;
		public DirectionsPage()
		{
			InitializeComponent();
			geoCoder = new Geocoder();

			var position = GetLocationAsync().Result;

			// Real coord locator - uncomment when ready
			//webView.Source = $"https://www.google.com/maps/dir/?q= {position.Latitude}, {position.Longitude}";
			
			// TODO: remove this:
			//webView.Source = $"https://www.google.com/maps/dir/?q= 39.9220717, -85.9815329";

			// TODO: Get data from storage
			JobDto nextJob = new JobDto();
			TruckDto truckRegistration = CredentialsManager.GetLoginCredentials();
			if (truckRegistration.TruckId == 0)
			{
				// Redirect to the RegisterLoginPage page. For some reason we don't have credentials, but we should by the time we get here.
				Navigation.PopToRootAsync().RunSynchronously();
				Navigation.PushAsync(new RegisterLoginPage()).RunSynchronously();
			}
			else
			{
				ApiClient.ApiClient client = new ApiClient.ApiClient(truckRegistration.TruckNumber, truckRegistration.ApiKey);
				nextJob = client.GetNextJob(truckRegistration.TruckId);
				if (nextJob != null && nextJob.JobId > 0)
				{
					lblPropertyName.Text = nextJob.PropertyName;
					lblPropertyAddress.Text = (nextJob.Street1 ?? "") + " " + (nextJob.Street2 ?? "") + "n\n" +
						" " + (nextJob.City ?? "") + ", " + (nextJob.State ?? "") + " " + (nextJob.ZipCode ?? "");

					lblOpsContactName.Text = nextJob.OperationsContactName ?? "";
					lblOpsContactPhone.Text = nextJob.SalesRepPhone ?? "";

					lblServiceName.Text = nextJob.JobServiceName ?? "";
					lblAccountManager.Text = nextJob.SalesRepContactName ?? "";
					// lblAccountManagerPhone.Text = nextJob.SalesRepPhone ?? "";

					if (nextJob.GpsLatitude != 0.0 && nextJob.GpsLongitude != 0.0)
					{
						webView.Source = $"https://www.google.com/maps?saddr=My+Location&daddr={nextJob.GpsLatitude},{nextJob.GpsLongitude}";
					}
					else
					{
						webView.Source = $"https://www.google.com/maps?saddr=My+Location&daddr={nextJob.Street1}+{nextJob.City}+{nextJob.State}+{nextJob.ZipCode}";
					}
					
				}
				else
				{
					// TODO: Should we display a modal with the nextJob.SpecialInstructions in it and allow them to re-try, 
					// TODO: or should we stay on this page and add a Retry button??
					lblOpsContactName.Text = "";
					lblPropertyAddress.Text = "";
					lblOpsContactPhone.Text = "";
					lblServiceName.Text = nextJob?.SpecialInstructions;
					lblAccountManager.Text = "";
					lblPropertyName.Text = "No More Properties";

					position = GetLocationAsync().Result;
					webView.Source = $"https://www.google.com/maps/dir/?q= {position.Latitude}, {position.Longitude}";
				}
			}
		}
		
		public async Task<Position> GetLocationAsync()
		{
			var position = await Geolocation.GetLastKnownLocationAsync();
			if (position != null)
			{
				return new Position(position.Latitude, position.Longitude);
			}
			return new Position();
		}

		void webOnNavigating(object sender, WebNavigatingEventArgs e)
		{
			progress.IsVisible = true;

		}

		void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
		{
			progress.IsVisible = false;
		}

		async void OnBackButtonClicked(object sender, EventArgs e)
		{
			if (webView.CanGoBack)
			{
				webView.GoBack();
			}
			else
			{
				await Navigation.PopAsync();
			}
		}

		void OnForwardButtonClicked(object sender, EventArgs e)
		{
			if (webView.CanGoForward)
			{
				webView.GoForward();
			}
		}
		
		private async void ButtonSiteMapClick(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SiteMapPage());
		}

		private async void ButtonRegisterTruckClick(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new RegisterLoginPage());
		}

		private void BtnCall_Click(object sender, EventArgs e)
		{
			var call = CrossMessaging.Current.PhoneDialer;
			if (call.CanMakePhoneCall)
			{
				if (lblOpsContactPhone.Text != string.Empty)
				{
					call.MakePhoneCall(lblOpsContactPhone.Text);
				}
			}
		}
	}
}