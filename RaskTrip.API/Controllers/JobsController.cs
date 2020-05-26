using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RaskTrip.DataAccess;
using RaskTrip.BusinessObjects.Models;
using System.Linq;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Net;
using NLog;
using Swashbuckle.Swagger.Annotations;
using System.Web;
using System.Text;
using RaskTrip.Utility.Security;

namespace RaskTrip.API.Controllers
{
	public class JobsController : ApiController
	{
		RaskTrip_Entities db = new RaskTrip_Entities();

		[SwaggerResponse(HttpStatusCode.OK, Type = typeof(JobDto))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[HttpGet]
		public IHttpActionResult GetNextJob(int truckId)
		{
			db.Configuration.ProxyCreationEnabled = false;
			// TODO: verify the auth header in the HttpRequest.
			// TODO: Create an ENUM for the TripRouteStatus values...
			const int TripRouteStatusEnumInProcess = 3;
			var job = db.Jobs.Include("Property").Include("PropertyAddress").Include("PropertyWorkType").Include("TripRoute.Trip")
				.OrderBy(r => r.TripRoute.TripRouteOrder).ThenBy(o => o.JobStopOrder)
				.Where(q => q.TripRoute.Trip.TruckId == truckId && q.TripRoute.IsPublished 
						&& q.TripRoute.TripStatusId <= TripRouteStatusEnumInProcess  && q.TripStatusId <= TripRouteStatusEnumInProcess)
				.FirstOrDefault(j => j.ActualTruckId == truckId);
			JobDto jobDto = new JobDto();
			if (job != null)
			{
				jobDto.JobId = job.JobId;
				jobDto.JobServiceName = job.JobServiceName;
				jobDto.PropertyName = job.PropertyName;
				jobDto.Street1 = job.PropertyAddress.Street1;
				jobDto.Street2 = job.PropertyAddress.Street2;
				jobDto.City = job.PropertyAddress.City;
				jobDto.State = db.States.FirstOrDefault(s => s.StateId == job.PropertyAddress.StateId).StateName;
				jobDto.ZipCode = job.PropertyAddress.ZipCode;
				jobDto.GpsLatitude = (job.PropertyAddress.GpsLatitude.HasValue) ? job.PropertyAddress.GpsLatitude.Value : 0.0;
				jobDto.GpsLongitude = (job.PropertyAddress.GpsLongitude.HasValue) ? job.PropertyAddress.GpsLongitude.Value : 0.0;
				jobDto.GpsRadius = (job.PropertyAddress.GpsRadius.HasValue) ? job.PropertyAddress.GpsRadius.Value : 0.0;
				jobDto.JobRequiresWeighInOut = job.JobRequiresWeighInOut;
				jobDto.SiteFotosUrl = job.Property.SitefotosUrl;

				if (job.SalesRepUserId.HasValue)
				{
					var salesRep = db.Users.FirstOrDefault(u => u.UserId == job.SalesRepUserId.Value);
					if (salesRep != null)
					{
						jobDto.SalesRepContactName = salesRep.FirstName + " " + salesRep.LastName;
						jobDto.SalesRepPhone = salesRep.MobilePhone;
					}
				}

				if (job.OperationsUserId.HasValue)
				{
					var operationsUser = db.Users.FirstOrDefault(u => u.UserId == job.OperationsUserId.Value);
					if (operationsUser != null)
					{
						jobDto.OperationsContactName = operationsUser.FirstName + " " + operationsUser.LastName;
						jobDto.OperationsContactPhone = operationsUser.MobilePhone;
					}
				}

				if (job.PropertyWorkType != null && !string.IsNullOrEmpty(job.PropertyWorkType.Notes))
					jobDto.SpecialInstructions = job.PropertyWorkType.Notes;
				else
					jobDto.SpecialInstructions = "";
			}
			else
			{
				return NotFound();
			}
			// TODO: update the job.TripStatusId = TripStatusEnum.Dispatched.GetHashCode(); and save.
			var result = Ok(jobDto);

			return result;
		}

		//[HttpPost]
		//[AllowAnonymous]
		public HttpResponseMessage PostRegisterTruck([FromBody] TruckDto truckDto)
		{
			Truck truck = null;
			User user = null;
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				if (truckDto != null)
				{
					if (!String.IsNullOrEmpty(truckDto.TruckNumber) && !String.IsNullOrEmpty(truckDto.ApiKey))
					{
						var username = $"TRUCK_{truckDto.TruckNumber}";
						truck = db.Trucks.FirstOrDefault(t => t.TruckNumber == truckDto.TruckNumber);

						if (truck != null && VerifyAuthentication(username, truckDto.ApiKey, truck.TruckId))
						{
							user = db.Users.FirstOrDefault(u => u.UserId == truck.TripApiUserId.Value);
							if (user != null)
							{
								user.LastLoginDate = DateTime.Now;
								user.FailedPasswordAttemptCount = 0;
								db.SaveChanges();
								truckDto.TruckId = truck.TruckId;
								truckDto.Message = "Successful Registration";
								message = Request.CreateResponse<TruckDto>(HttpStatusCode.Created, truckDto);
								//return Ok(response);
							}
						}
						else
						{
							message.StatusCode = HttpStatusCode.Unauthorized;
							message.ReasonPhrase = "Registration for this truck failed. Verify your Truck Number and API Key.";
						}
					}
					else
					{
						message.StatusCode = HttpStatusCode.BadRequest;
						message.ReasonPhrase = "Please provide a Truck Number and Api Key";
					}
				}
			}
			catch (Exception ex)
			{
				//Loggerlog(ex);  - put in a helper
				message.StatusCode = HttpStatusCode.InternalServerError;
				message.ReasonPhrase = "The registration request is either invalid or the system encountered an error when processing it";
			}
			return message;
		}

		//[HttpPost]
		//public void PostClockIn(StringContent jobData)
		//{
		//	var clockInTime = DateTime.Now;
		//	// Work with team/Dave P on data points
		// TODO: verify basic authentication from http header (write a common method to do this)
		// TODO: get the job from the database identified by the jobDto.JobId  -- include TripRoute and Trip
		// TODO: verify that it's OK to update the job: if job.TripStatusId == TripStatusEnum.Dispatched.GetHashCode() and the job is for this truckId
		// TODO: update the following job properties: 
		// TODO: ActualDriverVendorWorkerId (from Trip.DriverVendorWorkerId)
		// TODO: update ActualClockIn, if (JobRequiresWeighIn) update ActualWeightInOut, update TripStatusId = TripStatusEnum.InProcess
		// TODO: save the job.
		//}

		//[HttpPost]
		//public void PostClockOut(StringContent jobData)
		//{
		//	var clockOutTime = DateTime.Now;
		//	// Work with team/Dave P on data points
		// TODO: I would create a smaller, ClockOutDto that has only the properties necessary to complete this transaction...
		// TODO: verify basic authentication from http header (write a common method to do this)
		// TODO: get the job from the database identified by the jobDto.JobId  -- include TripRoute and Trip
		// TODO: verify that it's OK to update the job: if job.TripStatusId == TripStatusEnum.InProcess.GetHashCode() and the job is for this truckId
		// TODO: Find the PropertyFlatRate corresponding to the JobPropertyFlatRateId.  Lookup the "sibling" if the Dto's JobServiceName is different
		// TODO: determine which TripStatusId to use: SkippedNotPlowed, SkippedNoAccess, or Completed
		// TODO: update the following job properties:
		// TODO: ActualClockOut, ActualWeightOut and compute ActualTonnage (ifJobRequiresWeighInOut), ActualPropertyFlatRateId, TripStatusId
		// TODO: save the job.
		//}

		/// <summary>
		/// Verifies the basic authentication credentials passed in the HttpRequest header. The format of the basic auth is illustrated by the 
		/// following code, which a client would use to add the header to a request:
		/// request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(UserName + ":" + Password)));
		/// </summary>
		/// <param name="truckId"></param>
		/// <returns></returns>
		private bool VerifyBasicAuthCredentials(int truckId)
		{
			bool result = false;
			var request = HttpContext.Current.Request;
			if (request.Headers.HasKeys() && !String.IsNullOrEmpty(request.Headers.Get("Authorization")))
			{
				string basicAuthHeader = request.Headers.Get("Authorization");
				string basicAuthBase64 = (!String.IsNullOrEmpty(basicAuthHeader) && basicAuthHeader.StartsWith("Basic ")) ? basicAuthHeader.Replace("Basic ", "") : String.Empty;
				var basicAuthBinary = Convert.FromBase64String(basicAuthBase64);
				var basicAuth = ASCIIEncoding.ASCII.GetString(basicAuthBinary);
				var username = basicAuth.Split(':')[0];
				var password = basicAuth.Split(':')[1];
				return VerifyAuthentication(username, password, truckId);
			}
			return result;
		}

		private bool VerifyAuthentication(string username, string password, int truckId)
		{
			bool result = false;
			var user = db.Users.FirstOrDefault(u => u.Username == username && u.IsActive && !u.IsLockedOut);
			if (user != null)
			{
				var clearPassword = PasswordHelper.DecryptPassword(user.Password);
				result = clearPassword == password;

				var truck = db.Trucks.FirstOrDefault(t => t.TripApiUserId == user.UserId);
				result = result && truck != null && truck.TruckId == truckId;
			}
			return result;
		}
	}
}
