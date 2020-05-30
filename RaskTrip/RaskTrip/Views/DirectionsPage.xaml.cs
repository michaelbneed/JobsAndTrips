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
using System.Web;

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
				Navigation.PopToRootAsync();
				Navigation.PushAsync(new RegisterLoginPage());
			}
			else
			{
				ApiClient.ApiClient client = new ApiClient.ApiClient(truckRegistration.TruckNumber, truckRegistration.ApiKey);
				nextJob = client.GetNextJob(truckRegistration.TruckId);
				if (nextJob != null && nextJob.JobId > 0)
				{
					lblPropertyName.Text = nextJob.PropertyName;
					lblPropertyAddress.Text = $"{nextJob.Street1 ?? ""} {nextJob.Street2 ?? ""}\n {nextJob.City ?? ""}, {nextJob.State ?? ""} {nextJob.ZipCode ?? ""}";

					lblOpsContactName.Text = "Operations: " + (nextJob.OperationsContactName ?? "");
					lblOpsContactPhone.Text = nextJob.OperationsContactPhone ?? "";
					BtnCallOps.IsVisible = !string.IsNullOrEmpty(nextJob.OperationsContactPhone);

					lblServiceName.Text = nextJob.JobServiceName ?? "";

					lblAccountManagerName.Text = "Sales: " + (nextJob.SalesRepContactName ?? "");
					lblAccountManagerPhone.Text = nextJob.SalesRepPhone ?? "";
					BtnCallAccountManager.IsVisible = !string.IsNullOrEmpty(nextJob.SalesRepPhone);

					if (nextJob.GpsLatitude != 0.0 && nextJob.GpsLongitude != 0.0)
					{
						position = GetLocationAsync().Result;
						var here = $"{position.Latitude}%2C{position.Longitude}";
						var there = $"{nextJob.GpsLatitude}%2C{nextJob.GpsLongitude}";
						webView.Source = $"https://www.google.com/maps/dir/?api=1&origin={here}&destination={there}&travelmode=driving&dir_action=navigate";

						//webView.Source = $"https://www.google.com/maps/dir/?api=1&q={nextJob.GpsLatitude},{nextJob.GpsLongitude}";

						btnNavigate.CommandParameter = HttpUtility.UrlEncode($"{nextJob.GpsLatitude}|{nextJob.GpsLongitude}|{nextJob.PropertyName}");
					}
					else
					{
						var dest = HttpUtility.UrlEncode($"{nextJob.Street1}+{nextJob.City}+{nextJob.State}+{nextJob.ZipCode}");
						webView.Source = $"https://www.google.com/maps/dir/?api=1&origin=My+Location&destination={dest}";
						//webView.Source = $"https://www.google.com/maps?saddr=My+Location&daddr={nextJob.Street1}+{nextJob.City}+{nextJob.State}+{nextJob.ZipCode}";
					}
				}
				else
				{
					// TODO: Should we display a modal with the nextJob.SpecialInstructions in it and allow them to re-try, 
					// TODO: or should we stay on this page and add a Retry button??
					lblOpsContactName.Text = "";
					lblOpsContactPhone.Text = "";
					lblPropertyAddress.Text = "";
					lblServiceName.Text = nextJob?.SpecialInstructions;
					lblAccountManagerName.Text = "";
					lblAccountManagerPhone.Text = "";
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
		
		private async void btnNavigate_Click(object sender, EventArgs e)
		{
			var target = HttpUtility.UrlDecode((sender as Button)?.CommandParameter?.ToString())?.Split('|');
			if (target.Length == 3)
			{
				var mapOptions = new MapLaunchOptions() { Name = target[2], NavigationMode = NavigationMode.Driving};
				await Xamarin.Essentials.Map.OpenAsync(double.Parse(target[0]), double.Parse(target[1]), mapOptions);
			}
		}
		private async void ButtonSiteMap_Click(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SiteMapPage());
		}

		private void BtnCallOps_Click(object sender, EventArgs e)
		{
			InitiatePhoneCall(lblOpsContactPhone.Text);
		}
		private void BtnCallAccountManager_Click(object sender, EventArgs e)
		{
			InitiatePhoneCall(lblAccountManagerPhone.Text);
		}

		private void InitiatePhoneCall( string phoneNumber )
		{
			var call = CrossMessaging.Current.PhoneDialer;
			if (call.CanMakePhoneCall)
			{
				if (!string.IsNullOrEmpty(phoneNumber))
				{
					call.MakePhoneCall(phoneNumber);
				}
			}
		}
	}
}