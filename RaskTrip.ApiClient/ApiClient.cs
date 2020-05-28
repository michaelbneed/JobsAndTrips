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
		private string TripApiUrlBase = "https://dev3.adaptivesys.com/RaskTripApi-DEV/api/Jobs/";
		// private string TripApiUrlBase = "http://10.0.2.2:56596/api/Jobs/";
		// private string TripApiUrlBase = "https://10.0.2.2:56596/api/Jobs/";

		private string Username = "";
		private string ApiKey = "";

		#region Constructors
		public ApiClient() { }

		public ApiClient(string username, string apiKey)
		{
			Username = username;
			ApiKey = apiKey;
		}
		#endregion

		#region GetNextJob
		public JobDto GetNextJob(TruckDto truckRegistration)
		{

			// TEST IN BROWSER -  http://localhost:56596/api/Jobs/GetNextJob?truckId=1

			JobDto job = new JobDto();

			try
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(TripApiUrlBase);
					var response = client.GetStringAsync($"GetNextJob?truckId={truckRegistration.TruckId}");
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
		#endregion

		#region RegisterTruckAsync
		public async Task<TruckDto> PostRegisterTruckAsync(TruckDto truckRegistration)
		{
			try
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(TripApiUrlBase);
					var request = GetRequestHeaders(HttpMethod.Post, "PostRegisterTruck");
					SetRequestContent<TruckDto>(request, truckRegistration);
					var result = client.SendAsync(request).Result;
					
					if (result.IsSuccessStatusCode)
					{
						string serializedJson = result.Content.ReadAsStringAsync().Result;
						TruckDto registeredTruck = JsonConvert.DeserializeObject<TruckDto>(serializedJson);
						return await Task.FromResult(registeredTruck);
					}
					else
					{
						truckRegistration.Message = $"Truck Registration Failed with status {result.StatusCode} and reason {result.ReasonPhrase}";
					}
				}
			}
			catch (Exception ex)
			{
				//Logger.Log(ex);  log to a file on HD - put in a helper
				truckRegistration.Message = "Truck Registration Failed: " + ex.Message;
				truckRegistration.TruckId = 0;
			}
			return truckRegistration;
		}
        #endregion

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

        private HttpRequestMessage GetRequestHeaders(HttpMethod httpMethod, string url)
		{
			var request = new HttpRequestMessage(httpMethod, url);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(ApiKey))
				request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Username + ":" + ApiKey)));
			return request;
		}

		private void SetRequestContent<T>(HttpRequestMessage request, T content)
		{
			var json = JsonConvert.SerializeObject(content);
			request.Content = new StringContent(json, Encoding.UTF8, "application/json");
		}
	}
}
