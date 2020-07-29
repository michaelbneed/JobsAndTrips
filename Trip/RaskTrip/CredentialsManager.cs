using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;
using Trip.ApiClient;
using Trip.BusinessObjects.Models;
using Xamarin.Essentials;

namespace Trip
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
		//public static async Task<string> SaveLoginCredentials(TruckDto truckCredentials)
		public static string SaveLoginCredentials(TruckDto truckCredentials)
		{
			string message = $"Truck {truckCredentials.TruckNumber} was successfully registered and saved to your device.";
			try
			{
				//await Task.Run(() => { 
					SecureStorage.SetAsync("TruckId", truckCredentials.TruckId.ToString());
					SecureStorage.SetAsync("TruckNumber", truckCredentials.TruckNumber);
					SecureStorage.SetAsync("TruckApiKey", truckCredentials.ApiKey);
				//});
			}
			catch (Exception ex)
			{
				message = "Error: Registration credentials could not be saved: " + ex.Message;
			}
			return message;
		}

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
		/// Returns saved login credentials in a TruckDto object.
		/// Note: async await calls to secure storage were hanging -- so work-around is encapsulating synchronous calls in Task.Run.
		/// Note: Task.Run hangs on first SecureStorageGetAsync call inside the Task.Run. Tried with and without .ConfigureAwait(false)
		/// </summary>
		/// <returns>TruckDto with non-zero TruckId if there are saved credentials. TruckId = 0 otherwise.</returns>
		public static async Task<TruckDto> GetLoginCredentialsAsync()
		{
			TruckDto truckCreds = new TruckDto();
			try
			{
				truckCreds = await Task.Run<TruckDto>(() => { 
					string truckid = SecureStorage.GetAsync("TruckId").Result;
					int intTruckId = 0;
					if (int.TryParse(truckid, out intTruckId))
						truckCreds.TruckId = intTruckId;
					else
						truckCreds.TruckId = 0;

					truckCreds.TruckNumber = SecureStorage.GetAsync("TruckNumber").Result;
					truckCreds.ApiKey = SecureStorage.GetAsync("TruckApiKey").Result;
					return truckCreds;
				});
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
		public static async Task<TruckDto> VerifyCredentials(TruckDto truckCredentials)
		{
			ApiClient.ApiClient client = new ApiClient.ApiClient();
			var result = await client.RegisterTruckAsync(truckCredentials).ConfigureAwait(false);
			if (result != null)
			{
				return result;
			}
			else
			{
				result = new TruckDto();
				result.Message = "System or Network Error: Could not Verify the registration credentials. Please try again or report the problem if it continues.";
				result.TruckId = 0;
				return result;
			}
		}
	}
}
