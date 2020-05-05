using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RaskTrip.BusinessObjects.Models;
using System.IO;
using Newtonsoft.Json;
using System.Net.Security;


namespace RaskTrip.ApiClient
{
	public class ApiClient
	{
		//static HttpClient httpClient = new HttpClient();

		//HttpClient httpClient = new HttpClient();

		public JobDto GetNextJob(Truck truckRegistration)
		{
			var intTruckId = Convert.ToInt32(truckRegistration.TruckId);

			string url = $"http://10.0.2.2:56596/api/Jobs/";
			string ssl = $"https://localhost:44357/api/Jobs/";

			// TEST IN BROWSER -  http://localhost:56596/api/Jobs/GetNextJob?truckId=1

			JobDto job = new JobDto();


			try
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(url);
					var response = client.GetStringAsync($"GetNextJob?truckId={intTruckId}");

					job = JsonConvert.DeserializeObject<JobDto>(response.Result);

				}
			}
			catch (HttpRequestException ex)
			{
				var error = ex.Message;
				throw;
			}
			catch (AggregateException ex)
			{
				var error = ex.Message;
				throw;
			}

			return job;
		}

		public async Task<bool> PostRegisterTruck(Truck truckRegistration)
		{
			string url = $"https://localhost:44357/api/Jobs/PostRegisterTruck"; 

			var jsonTruckRegistration = JsonConvert.SerializeObject(truckRegistration);
			var data = new StringContent(jsonTruckRegistration, Encoding.UTF8, "application/json");

			//truckRegistration.TruckId = 5;
			//truckRegistration.TripApiUserId = 9;
			//truckRegistration.TruckNumber = "123456";

			using (var client = new HttpClient())
			{
				try
				{
					var response = await client.PostAsync(url, data);
					var result = response.Content.IsHttpResponseMessageContent();
					return result;
				}
				catch (Exception ex)
				{
					var error = ex.Message;
					throw;
				}
				
			}				
		}

		//public async Task<bool> PostClockInAsync(Job job)
		//{
		//	string url = $"https://localhost:44357/api/Jobs/PostClockIn"; // what will my api url look like - move to config file	

		//	var jsonJob = JsonConvert.SerializeObject(job);
		//	var data = new StringContent(jsonJob, Encoding.UTF8, "application/json");
		//	var response = await httpClient.PostAsync(url, data);

		//	var result = response.Content.IsHttpResponseMessageContent();
		//	return result;
		//}

		//public async Task<bool> PostClockOut(Job job)
		//{
		//	string url = $"https://localhost:44357/api/Jobs/PostClockOut"; // what will my api url look like - move to config file	

		//	var jsonJob = JsonConvert.SerializeObject(job);
		//	var data = new StringContent(jsonJob, Encoding.UTF8, "application/json");
		//	var response = await httpClient.PostAsync(url, data);

		//	var result = response.Content.IsHttpResponseMessageContent();
		//	return result;
		//}
	}
}
