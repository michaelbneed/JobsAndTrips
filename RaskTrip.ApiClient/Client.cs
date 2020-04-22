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

// TODO: Move into Client project - class lib
namespace RaskTrip.ApiClient
{
	public class Client
	{
		static HttpClient client = new HttpClient();

		public async Task<Job> GetNextJob(TruckRegistration truckRegistration)
		{
			var intTruckId = Convert.ToInt32(truckRegistration.TruckId);
			
			string url = $"https://localhost:44357/api/"; // base uri
			//string url = $https://localhost:44357/api/Jobs/GetNextJob?truckId=1234"; // what will my api url look like - move to config file

			Job newJob = new Job();

			try
			{
				client.BaseAddress = new Uri(url);
				//client.DefaultRequestHeaders.Accept.Clear();
				//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				using (var response = await client.GetAsync("Jobs"))
				{
					var resonseCode = response.StatusCode;
					newJob = await response.Content.ReadAsAsync<Job>();

					if (response.IsSuccessStatusCode)
					{
						newJob = await response.Content.ReadAsAsync<Job>();

					}
				}
			}
			catch (HttpRequestException ex)
			{
				ex.Message.ToString();
			}

			//using (var client = new HttpClient())
			//{
			//	try
			//	{
			//		client.BaseAddress = new Uri(url);
			//		client.DefaultRequestHeaders.Accept.Clear(); 
			//		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			//		HttpResponseMessage response = client.PostAsync(url, new StringContent(intTruckId.ToString())).Result;
			//		var resonseCode = response.StatusCode;
			//		newJob = await response.Content.ReadAsAsync<Job>();

			//		if (response.IsSuccessStatusCode)
			//		{
			//			newJob = await response.Content.ReadAsAsync<Job>();
						
			//		}
			//	}
			//	catch (HttpRequestException ex)
			//	{
			//		ex.Message.ToString();
			//	}
				
			//}

			return newJob;


		}

		public async Task<bool> PostRegisterTruck(TruckRegistration truckRegistration)
		{
			string url = $"https://localhost:44357/api/Jobs/PostRegisterTruck"; // what will my api url look like - move to config file	
			
			var jsonTruckRegistration = JsonConvert.SerializeObject(truckRegistration);
			var data = new StringContent(jsonTruckRegistration, Encoding.UTF8, "application/json");
			var response = await client.PostAsync(url, data);

			var result = response.Content.IsHttpResponseMessageContent();
			return result;
		}

		public async Task<bool> PostClockInAsync(Job job)
		{
			string url = $"https://localhost:44357/api/Jobs/PostClockIn"; // what will my api url look like - move to config file	
			
			var jsonJob = JsonConvert.SerializeObject(job);
			var data = new StringContent(jsonJob, Encoding.UTF8, "application/json");
			var response = await client.PostAsync(url, data);

			var result = response.Content.IsHttpResponseMessageContent();
			return result;
		}

		public async Task<bool> PostClockOut(Job job)
		{
			string url = $"https://localhost:44357/api/Jobs/PostClockOut"; // what will my api url look like - move to config file	

			var jsonJob = JsonConvert.SerializeObject(job);
			var data = new StringContent(jsonJob, Encoding.UTF8, "application/json");
			var response = await client.PostAsync(url, data);

			var result = response.Content.IsHttpResponseMessageContent();
			return result;
		}
	}
}
