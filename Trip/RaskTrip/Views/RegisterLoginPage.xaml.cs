using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trip.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Messaging;
using Trip.ApiClient;
using Trip.BusinessObjects.Models;
using System.Text;

namespace Trip.Views
{
	public partial class RegisterLoginPage : ContentPage
	{
		public RegisterLoginPage()
		{
			InitializeComponent();
		}

		private async void ButtonRegisterTruck(object sender, EventArgs e)
		{
			string message = "Attempting to register the truck";
			messageLabel.Text = message;
			
			TruckDto newTruck = new TruckDto();
			newTruck.TruckNumber = usernameEntry.Text;
			newTruck.ApiKey = passwordEntry.Text;

			var result = await TripContext.VerifyCredentials(newTruck);
			if (result != null)
			{
				message = result.Message;
				if (result.TruckId > 0)
				{
					//message = await CredentialsManager.SaveLoginCredentials(result);
					message = CredentialsManager.SaveLoginCredentials(result);
					if (!message.StartsWith("Error:"))
					{
						messageLabel.Text = message;
						TripContext.Credentials = result;
						var jobChanged = await TripContext.GetNextJob();
						TripContext.CurrentPage = "DirectionsPage";
						await Navigation.PushAsync(new DirectionsPage());
					}
				}
			}
			else
				message += " failed";
			messageLabel.Text = message;
		}
	}
}