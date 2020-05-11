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
using RaskTrip.Helpers;
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
			byte[] pwBytes = Encoding.ASCII.GetBytes(passwordEntry.Text);

			var passwordToSave = PasswordHelper.EncryptPassword(pwBytes, true);
			//newTruck.ApiKey = passwordEntry.Text;  -- // Change data type

			client.PostRegisterTruckAsync(newTruck);
		}
	}
}