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

		private async void ButtonRegisterTruck(object sender, EventArgs e)
		{
			string message = "Attempting to register the truck";
			messageLabel.Text = message;

			ApiClient.ApiClient client = new ApiClient.ApiClient();

			TruckDto newTruck = new TruckDto();

			newTruck.TruckNumber = usernameEntry.Text;
			newTruck.ApiKey = passwordEntry.Text;
			var result = await client.PostRegisterTruckAsync(newTruck);
			if (result != null)
			{
				message = result.Message;
				if (result.TruckId > 0)
				{
					message = await SaveLoginCredentials(result.TruckId, result.TruckNumber, result.ApiKey);
				}
			}
			else
				message += " failed";
			messageLabel.Text = message;
		}

		private async Task<string> SaveLoginCredentials(int truckId, string truckNumber, string apiKey)
		{
			string message = $"Truck {truckNumber} was successfully registered and saved to your device.";
			try
			{
				await SecureStorage.SetAsync("TruckId", truckId.ToString());
				await SecureStorage.SetAsync("TruckNumber", truckNumber);
				await SecureStorage.SetAsync("TruckApiKey", apiKey);
			}
			catch(Exception ex)
			{
				message = "Registration credentials could not be saved: " + ex.Message;
			}
			return message;
		}
		
		private async Task<TruckDto> GetLoginCredentials()
		{
			TruckDto truckCreds = new TruckDto();
			try
			{
				string truckid = await SecureStorage.GetAsync("TruckId");
				truckCreds.TruckId = int.Parse(truckid);

				truckCreds.TruckNumber = await SecureStorage.GetAsync("TruckNumber");
				truckCreds.ApiKey = await SecureStorage.GetAsync("TruckApiKey");
			}
			catch (Exception ex)
			{
				truckCreds.Message = "Saved Credentials Not Available: " + ex.Message;
			}
			return truckCreds;
		}
	}
}