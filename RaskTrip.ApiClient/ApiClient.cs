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
		public async Task<JobDto> GetNextJob(TruckDto truckRegistration)
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
					
					var response = await client.GetAsync($"GetNextJob?truckId={intTruckId}");
					response.EnsureSuccessStatusCode();
					string content = await response.Content.ReadAsStringAsync();

					return await Task.FromResult(job);
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

		public TruckDto PostRegisterTruckAsync(TruckDto truckRegistration)
		{
			//string url = $"https://localhost:44357/api/Jobs/PostRegisterTruck";
			string url = $"http://10.0.2.2:56596/api/Jobs/PostRegisterTruck";

			//var jsonTruckRegistration = JsonConvert.SerializeObject(truckRegistration);
			//var data = new StringContent(jsonTruckRegistration, Encoding.UTF8, "application/json");
			try
			{
				var json = JsonConvert.SerializeObject(truckRegistration);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var client = new HttpClient();

				//client.DefaultRequestHeaders.Add("ContentType", "application/json");

				var result = client.PostAsync($"{url}", content);

				//client.Dispose();

				return truckRegistration;
			}
			catch (Exception ex)
			{
				//Logger.Log(ex);  log to a file on HD - put in a helper
				throw;
			}
			

			//using (var client = new HttpClient())
			//{
			//	client.PostAsJsonAsync(url, truckRegistration);

			//	//var content = new StringContent(truckRegistration.ToString(), Encoding.UTF8, "application/json");
			//	//content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			//	//var result = client.PostAsync(url, content).Result;

			//	//var content = new StringContent(JsonConvert.SerializeObject(jsonTruckRegistration), System.Text.Encoding.UTF8, "application/json");

			//	// content = new HttpContent(truckRegistration);

			//	//var result = client.PostAsJsonAsync(url, truckRegistration).Result;
			//	return truckRegistration;


			//	//using (var content = new StringContent(JsonConvert.SerializeObject(jsonTruckRegistration), System.Text.Encoding.UTF8, "application/json"))
			//	//{
			//	//	try
			//	//	{
			//	//		HttpResponseMessage result = client.PostAsync(url, content).Result;

			//	//		if (result.StatusCode == System.Net.HttpStatusCode.Created)
			//	//			return truckRegistration;

			//	//		string returnValue = result.Content.ReadAsStringAsync().Result;

			//	//		throw new Exception($"Failed to POST data: ({result.StatusCode}): {returnValue}");
			//	//	}
			//	//	catch (Exception ex)
			//	//	{
			//	//		var error = ex.Message;
			//	//		throw;
			//	//	}
			//	//}
			//}				
		}

		//public async Task<bool> PostLoginTruck(TruckDto truckRegistration)
		//{
		//	//string url = $"https://localhost:44357/api/Jobs/PostLoginTruck";
		//	string url = $"http://10.0.2.2:56596/api/Jobs/PostLoginTruck";

		//	var jsonTruckRegistration = JsonConvert.SerializeObject(truckRegistration);
		//	var data = new StringContent(jsonTruckRegistration, Encoding.UTF8, "application/json");

		//	using (var client = new HttpClient())
		//	{
		//		try
		//		{
		//			client.BaseAddress = new Uri(url);

		//			//client.DefaultRequestHeaders.Add("ContentType", "application/json");

		//			var userToAuthorize = truckRegistration.TruckNumber;
		//			var passwordToUse = truckRegistration.ApiKey;

		//			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{userToAuthorize}:{passwordToUse}");
		//			string val = System.Convert.ToBase64String(plainTextBytes);
		//			client.DefaultRequestHeaders.Add("Authorization", "Basic " + val);

		//			var method = new HttpMethod("GET");

		//			HttpResponseMessage response = client.GetAsync(url).Result;
		//			//string content = string.Empty;

					
		//			//using (StreamReader stream = new StreamReader(response.Content.ReadAsStreamAsync().Result, System.Text.Encoding.GetEncoding()))
		//			//{
		//			//	content = stream.ReadToEnd();
		//			//}

		//			var result = response.Content.ReadAsStringAsync();
		//			return Task.FromResult(result);
		//		}
		//		catch (Exception ex)
		//		{
		//			var error = ex.Message;
		//			throw;
		//		}

		//	}
		//}

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
