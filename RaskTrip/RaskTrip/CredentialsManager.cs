using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;
using RaskTrip.ApiClient;
using RaskTrip.BusinessObjects.Models;
using Xamarin.Essentials;

namespace RaskTrip
{
    public static class CredentialsManager
    {
		/// <summary>
		/// Save the Truck credentials for later use in subsequent API calls. 
		/// </summary>
		/// <param name="truckId">unique identifier returned by PostRegisterTruck API call</param>
		/// <param name="truckNumber">The truck number entered by the user during truck registration</param>
		/// <param name="apiKey">The API key (password) entered by the user during truck registration</param>
		/// <returns>a message as to weather the credentials were saved</returns>
		public static string SaveLoginCredentials(TruckDto truckCredentials)
		{
			string message = $"Truck {truckCredentials.TruckNumber} was successfully registered and saved to your device.";
			try
			{
				SecureStorage.SetAsync("TruckId", truckCredentials.TruckId.ToString());
				SecureStorage.SetAsync("TruckNumber", truckCredentials.TruckNumber);
				SecureStorage.SetAsync("TruckApiKey", truckCredentials.ApiKey);
			}
			catch (Exception ex)
			{
				message = "Error: Registration credentials could not be saved: " + ex.Message;
			}
			return message;
		}

		/// <summary>
		/// Returns saved login credentials in a TruckDto object.
		/// </summary>
		/// <returns>TruckDto with non-zero TruckId if there are saved credentials. TruckId = 0 otherwise.</returns>
		public static TruckDto GetLoginCredentials()
		{
			TruckDto truckCreds = new TruckDto();
			try
			{
				string truckid = SecureStorage.GetAsync("TruckId").Result;
				int intTruckId = 0;
				if (int.TryParse(truckid, out intTruckId))
					truckCreds.TruckId = intTruckId;
				else
					truckCreds.TruckId = 0;

				truckCreds.TruckNumber = SecureStorage.GetAsync("TruckNumber").Result;
				truckCreds.ApiKey = SecureStorage.GetAsync("TruckApiKey").Result;
			}
			catch (Exception ex)
			{
				truckCreds.Message = "Saved Credentials Not Available: " + ex.Message;
			}
			return truckCreds;
		}

		/// <summary>
		/// Calls the PostRegisterTruck API in order to verify that the credentials are valid.
		/// </summary>
		/// <param name="truckCredentials"></param>
		/// <returns>the TruckDto -- TruckId > 0 for a successful login.</returns>
		public static TruckDto VerifyCredentials(TruckDto truckCredentials)
		{
			ApiClient.ApiClient client = new ApiClient.ApiClient();
			var result = client.RegisterTruck(truckCredentials);
			if (result != null)
			{
				return result;
			}
			else
				return new TruckDto();
		}
	}
}
