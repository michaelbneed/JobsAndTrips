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
	public class Client
	{
		//static HttpClient httpClient = new HttpClient();

		HttpClient httpClient = new HttpClient();

		public Job GetNextJob(Truck truckRegistration)
		{
			var intTruckId = Convert.ToInt32(truckRegistration.TruckId);

			string url = $"http://localhost:56596/api/Jobs/";
			//string ssl url = $"https://localhost:44357/api/Jobs/"; 

			// TEST IN BROWSER -  http://localhost:56596/api/Jobs/GetNextJob?truckId=1

			Job job = new Job();

			using (var client = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(url);
				var response = httpClient.GetStringAsync($"GetNextJob?truckId={intTruckId}").Result;

				job = JsonConvert.DeserializeObject<Job>(response);
			}

			return job;
		}

		public async Task<bool> PostRegisterTruck(Truck truckRegistration)
		{
			string url = $"https://localhost:44357/api/Jobs/PostRegisterTruck"; // what will my api url look like - move to config file	
			
			var jsonTruckRegistration = JsonConvert.SerializeObject(truckRegistration);
			var data = new StringContent(jsonTruckRegistration, Encoding.UTF8, "application/json");
			var response = await httpClient.PostAsync(url, data);

			var result = response.Content.IsHttpResponseMessageContent();
			return result;
		}

		public async Task<bool> PostClockInAsync(Job job)
		{
			string url = $"https://localhost:44357/api/Jobs/PostClockIn"; // what will my api url look like - move to config file	
			
			var jsonJob = JsonConvert.SerializeObject(job);
			var data = new StringContent(jsonJob, Encoding.UTF8, "application/json");
			var response = await httpClient.PostAsync(url, data);

			var result = response.Content.IsHttpResponseMessageContent();
			return result;
		}

		public async Task<bool> PostClockOut(Job job)
		{
			string url = $"https://localhost:44357/api/Jobs/PostClockOut"; // what will my api url look like - move to config file	

			var jsonJob = JsonConvert.SerializeObject(job);
			var data = new StringContent(jsonJob, Encoding.UTF8, "application/json");
			var response = await httpClient.PostAsync(url, data);

			var result = response.Content.IsHttpResponseMessageContent();
			return result;
		}
	}
}
