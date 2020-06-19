using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RaskTrip.Views;

namespace RaskTrip
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();
			//OnStart();
		}

		protected override void OnStart()
		{
			//MainPage.Navigation.PushAsync(new DirectionsPage());
			//var truckCredentials = CredentialsManager.GetLoginCredentials();
			//if (truckCredentials.TruckId > 0)
			//{
			//	var validCredentials = CredentialsManager.VerifyCredentials(truckCredentials);
			//	if (validCredentials.TruckId > 0)
			//		MainPage.Navigation.PushAsync(new DirectionsPage());
			//	else
			//		MainPage.Navigation.PushAsync(new RegisterLoginPage());
			//}
			//else
			//{
			//	MainPage.Navigation.PushAsync(new RegisterLoginPage());
			//}
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
