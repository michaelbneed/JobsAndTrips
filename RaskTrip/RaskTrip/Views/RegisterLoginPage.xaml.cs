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
using System.Text;

namespace RaskTrip.Views
{
	public partial class RegisterLoginPage : ContentPage
	{
		public RegisterLoginPage()
		{
			InitializeComponent();

		}

		private void ButtonRegisterTruck(object sender, EventArgs e)
		{
			string message = "Attempting to register the truck";
			messageLabel.Text = message;
			
			TruckDto newTruck = new TruckDto();
			newTruck.TruckNumber = usernameEntry.Text;
			newTruck.ApiKey = passwordEntry.Text;

			var result = CredentialsManager.VerifyCredentials(newTruck);
			if (result != null)
			{
				message = result.Message;
				if (result.TruckId > 0)
				{
					message = CredentialsManager.SaveLoginCredentials(result);
					if (!message.StartsWith("Error:"))
						Navigation.PushAsync(new DirectionsPage()).RunSynchronously();
				}
			}
			else
				message += " failed";
			messageLabel.Text = message;
		}

	}
}