﻿using System;
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
			webView.Source = $"https://www.google.com/maps/dir/?q= 39.9220717, -85.9815329";

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
				if (lblPropertyPhone.Text != string.Empty)
				{
					call.MakePhoneCall(lblPropertyPhone.Text);
				}
			}
		}
	}
}