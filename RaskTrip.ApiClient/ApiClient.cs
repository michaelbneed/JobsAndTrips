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

		public ApiClient(string truckNumber, string apiKey)
		{
			// enable Basic auth in API and client
			Username = truckNumber;
			ApiKey = apiKey;
		}
		#endregion

		#region GetNextJob
		/// <summary>
		/// Calls GetNextJob?truckId={truckId} to get the next (or currently incomplete) job.
		/// Emulator: 
		/// </summary>
		/// <param name="truckId"></param>
		/// <returns>A jobDto with JobId > 0 is a job to be displayed. A jobId of 0 has SpecialInstructions explaining why there isn't a next job.</returns>
		public JobDto GetNextJob(int truckId)
		{
			JobDto job = new JobDto();
			try
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(TripApiUrlBase);
					string requestUrl = $"GetNextJob?truckId={truckId}";
					var request = GetRequestHeaders(HttpMethod.Get, requestUrl);
					var result = client.SendAsync(request).Result;
					if (result.IsSuccessStatusCode)
					{
						string serializedJson = result.Content.ReadAsStringAsync().Result;
						job = JsonConvert.DeserializeObject<JobDto>(serializedJson);
					}
					else if (result.StatusCode == HttpStatusCode.NotFound)
					{
						job.JobId = 0;
						job.SpecialInstructions = "There are no more jobs scheduled for you at this time.";
					}
				}
			}
			catch (HttpRequestException ex)
			{
				job.JobId = 0;
				job.SpecialInstructions = $"The request to get the next job failed. Check your internet connectivity and try again: {ex.Message}";
			}
			catch (AggregateException ex)
			{
				job.JobId = 0;
				job.SpecialInstructions = $"An error occurred trying to get the next job. Check your internet connectivity and try again: {ex.Message}";
			}

			return job;
		}
		#endregion

		#region RegisterTruck
		public TruckDto RegisterTruck(TruckDto truckRegistration)
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
						return registeredTruck;
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

        #region ClockIn
        public bool ClockIn(ClockInDto clockInDto)
		{
			try
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(TripApiUrlBase);
					string requestUrl = $"PostClockIn";
					var request = GetRequestHeaders(HttpMethod.Get, requestUrl);
					var jsonContent = JsonConvert.SerializeObject(clockInDto);
					request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
					var result = client.SendAsync(request).Result;
					if (result.IsSuccessStatusCode)
					{
						string serializedJson = result.Content.ReadAsStringAsync().Result;
						clockInDto = JsonConvert.DeserializeObject<ClockInDto>(serializedJson);
						return true;
					}
					else
						return false;
				}
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				return false;
			}
		}
		#endregion

		#region ClockOut
		public bool ClockOut(ClockOutDto clockOutDto)
		{
			try
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(TripApiUrlBase);
					string requestUrl = $"PostClockOut";
					var request = GetRequestHeaders(HttpMethod.Get, requestUrl);
					var jsonContent = JsonConvert.SerializeObject(clockOutDto);
					request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
					var result = client.SendAsync(request).Result;
					if (result.IsSuccessStatusCode)
					{
						string serializedJson = result.Content.ReadAsStringAsync().Result;
						clockOutDto = JsonConvert.DeserializeObject<ClockOutDto>(serializedJson);
						return true;
					}
					else
						return false;
				}
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				return false;
			}
		}
		#endregion

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
