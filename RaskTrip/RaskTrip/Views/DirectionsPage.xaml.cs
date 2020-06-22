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
using System.Threading;

namespace RaskTrip.Views
{
	public partial class DirectionsPage : ContentPage
	{
		public JobDto CurrentJob
		{ get { return TripContext.CurrentJob; } set { TripContext.CurrentJob = value; } }

		public TruckDto Credentials
		{ get { return TripContext.Credentials; } set { TripContext.Credentials = value; } }

		public DirectionsPage()
		{
			InitializeComponent();
		
			// TODO: remove this:
			//webView.Source = $"https://www.google.com/maps/dir/?q= 39.9220717, -85.9815329";

			if (!CheckRegisterLoginRedirect())
			{
				BindPageFields();
			}
		}
		private bool CheckRegisterLoginRedirect()
		{
			if (Credentials.TruckId == 0)
			{
				// Redirect to the RegisterLoginPage page. For some reason we don't have credentials, but we should by the time we get here.
				Navigation.PopToRootAsync();
				Navigation.PushAsync(new RegisterLoginPage());
				return true;
			}
			else
				return false;
		}
		private void BindPageFields()
		{
			if (CurrentJob != null && CurrentJob.JobId > 0)
			{
				lblPropertyName.Text = CurrentJob.PropertyName;
				lblPropertyAddress.Text = $"{CurrentJob.Street1 ?? ""} {CurrentJob.Street2 ?? ""}\n {CurrentJob.City ?? ""}, {CurrentJob.State ?? ""} {CurrentJob.ZipCode ?? ""}";

				lblOpsContactName.Text = "Operations: " + (CurrentJob.OperationsContactName ?? "");
				lblOpsContactPhone.Text = CurrentJob.OperationsContactPhone ?? "";
				BtnCallOps.IsVisible = !string.IsNullOrEmpty(CurrentJob.OperationsContactPhone);

				lblServiceName.Text = CurrentJob.JobServiceName ?? "";

				lblAccountManagerName.Text = "Sales: " + (CurrentJob.SalesRepContactName ?? "");
				lblAccountManagerPhone.Text = CurrentJob.SalesRepPhone ?? "";
				BtnCallAccountManager.IsVisible = !string.IsNullOrEmpty(CurrentJob.SalesRepPhone);
			}
				
		}

		//public async Task<Position> GetLocationAsync()
		//{
		//	var position = await Geolocation.GetLastKnownLocationAsync();
		//	if (position != null)
		//	{
		//		return new Position(position.Latitude, position.Longitude);
		//	}
		//	return new Position();
		//}
		protected override async void OnAppearing()
		{
			if (CurrentJob != null && CurrentJob.JobId > 0 )
			{
				if (CurrentJob.GpsLatitude != 0.0 && CurrentJob.GpsLongitude != 0.0)
				{
					var location = await Geolocation.GetLastKnownLocationAsync();
					var here = $"{location.Latitude}%2C{location.Longitude}";
					//var here = "My+Location";
					var there = $"{CurrentJob.GpsLatitude}%2C{CurrentJob.GpsLongitude}";
					webView.Source = $"https://www.google.com/maps/dir/?api=1&origin={here}&destination={there}&travelmode=driving&dir_action=navigate";

					//webView.Source = $"https://www.google.com/maps/dir/?api=1&q={nextJob.GpsLatitude},{nextJob.GpsLongitude}";

					btnNavigate.CommandParameter = HttpUtility.UrlEncode($"{CurrentJob.GpsLatitude}|{CurrentJob.GpsLongitude}|{CurrentJob.PropertyName}");
				}
				else
				{
					var dest = HttpUtility.UrlEncode($"{CurrentJob.Street1}+{CurrentJob.City}+{CurrentJob.State}+{CurrentJob.ZipCode}");
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
				lblServiceName.Text = CurrentJob?.SpecialInstructions;
				lblAccountManagerName.Text = "";
				lblAccountManagerPhone.Text = "";
				lblPropertyName.Text = "No More Properties";

				var location = await Geolocation.GetLastKnownLocationAsync();
				webView.Source = $"https://www.google.com/maps/dir/?q= {location.Latitude}, {location.Longitude}";
				//webView.Source = "https://www.google.com/maps/dir/?q=My+Location";
			}
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
				//var watcher = new LocationArrivalWatcher(CurrentJob);
				//var watcherTask = watcher.Run().ConfigureAwait(true);
				await Xamarin.Essentials.Map.OpenAsync(double.Parse(target[0]), double.Parse(target[1]), mapOptions);
				//if (watcherTask.GetAwaiter().GetResult())
				//{
				//	await Navigation.PushAsync(new SiteMapPage());
				//}
			}
		}
		private async void ButtonSiteMap_Click(object sender, EventArgs e)
		{
			// TODO: Validate that the current location is within the arrival radius of the job's location
			// TODO: Remove subscription to receive location updates and check for arrival?

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