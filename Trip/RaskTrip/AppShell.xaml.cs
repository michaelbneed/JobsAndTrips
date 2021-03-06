﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Services;
using Xamarin.Forms.Xaml;
using Trip.Views;
using Trip.BusinessObjects;

namespace Trip
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
			
			SetTabBarIsVisible(this, false);
			SetNavBarIsVisible(this, false);
		}

		private async void ButtonDriveClick(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new DirectionsPage());
		}

		private async void ButtonSiteMapClick(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SiteMapPage());
		}
	}
}
