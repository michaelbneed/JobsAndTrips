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
			ApiClient.ApiClient client = new ApiClient.ApiClient();

			TruckDto newTruck = new TruckDto();

			newTruck.TruckNumber = usernameEntry.Text;
			newTruck.ApiKey = passwordEntry.Text;
			var result = client.PostRegisterTruckAsync(newTruck);
			// TODO: if the call is successful, then store the TruckId, TruckNumber, and ApiKey in Essentials KeyValuePair storage
			// TODO: failure:  display a message and stay on this page.
		}
	}
}