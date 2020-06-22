using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RaskTrip.Views;
using System.Threading.Tasks;
using RaskTrip.BusinessObjects.Models;
using RaskTrip.ApiClient;

namespace RaskTrip
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//MainPage = new SplashPage();
			MainPage = new AppShell();	
		}

		protected override async void OnStart()
		{
			var isValid = await TripContext.EstablishVerifiedCredentials();
			if (!isValid)
				await MainPage.Navigation.PushAsync(new RegisterLoginPage());
			OnResume();
		}

		protected override void OnSleep()
		{
			// Stop any background services...
		}

		protected override async void OnResume()
		{
			bool jobChanged = false;
			if (TripContext.Credentials != null && TripContext.Credentials.TruckId > 0)
			{
				// TODO: should we call GetNextJob unconditionally to see if the current job assignment has changed? 
				if (TripContext.CurrentJob == null || TripContext.CurrentJob.JobId == 0)
				{
					jobChanged = await TripContext.GetNextJob();
				}
				if (TripContext.CurrentJob != null)
				{
					if (string.IsNullOrEmpty(TripContext.CurrentPage) || TripContext.CurrentPage == "DirectionsPage" || jobChanged)
					{
						TripContext.CurrentPage = "DirectionsPage";
						await MainPage.Navigation.PushAsync(new DirectionsPage());
					}
					else if (TripContext.CurrentPage == "SiteMapPage")
						await MainPage.Navigation.PushAsync(new SiteMapPage());
				}
			}
		}
	}
}
