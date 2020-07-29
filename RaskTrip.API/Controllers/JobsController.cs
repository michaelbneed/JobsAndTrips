using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Trip.DataAccess;
using Trip.BusinessObjects.Models;
using Trip.BusinessObjects.Enums;
using System.Linq;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Net;
using NLog;
using Swashbuckle.Swagger.Annotations;
using System.Web;
using System.Text;
using Trip.Utility.Security;
using NLog.Fluent;
using System.Collections.Generic;
using System.Collections;
using System.ServiceModel.Channels;

namespace Trip.API.Controllers
{
	public class JobsController : ApiController
	{
		Trip_Entities db = new Trip_Entities();
		Enums enums = new Enums();

        #region GetNextJob
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(JobDto))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
		[SwaggerResponse(HttpStatusCode.NotFound)]
		[HttpGet]
		public IHttpActionResult GetNextJob(int truckId)
		{
			db.Configuration.ProxyCreationEnabled = false;

			if (VerifyBasicAuthCredentials(truckId))
			{
				var stausInProgress = Enums.TripStatusEnum.InProcess.GetHashCode();
				var job = db.Jobs.Include("Property")
					.Include("PropertyAddress")
					.Include("PropertyWorkType")
					.Include("TripRoute.Trip")
					.OrderBy(r => r.TripRoute.TripRouteOrder).ThenBy(o => o.JobStopOrder)
					.Where(q => q.TripRoute.Trip.TruckId == truckId && q.TripRoute.IsPublished
							&& q.TripRoute.TripStatusId <= stausInProgress && q.TripStatusId <= stausInProgress)
					.FirstOrDefault(j => j.ActualTruckId == truckId);
				JobDto jobDto = new JobDto();

				try
				{
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

						job.TripRoute.TripStatusId = Enums.TripStatusEnum.Dispatched.GetHashCode();
						job.TripStatusId = Enums.TripStatusEnum.Dispatched.GetHashCode();
						job.ActualTruckId = truckId;
						db.SaveChanges();
					}
					else
					{
						Log.Info("Failed to find a qualified job!");
						return NotFound();
					}

					return Ok(jobDto);
				}
				catch (Exception ex)
				{
					Log.Error("Error: " + ex.InnerException + "Message: " + ex.Message + "StackTrace: " + ex.StackTrace);
					throw;
				}
			}
			else
			{
				return Unauthorized();
			}			
		}
        #endregion

        #region PostRegisterTruck
        [HttpPost]
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
							}
						}
						else
						{
							Log.Info(HttpStatusCode.Unauthorized + " " + "Registration for this truck failed.");
							message.StatusCode = HttpStatusCode.Unauthorized;
							message.ReasonPhrase = "Registration for this truck failed. Verify your Truck Number and API Key.";
						}
					}
					else
					{
						Log.Info(HttpStatusCode.BadRequest + " " + "Bad Truck Number (" + truckDto.TruckNumber + ") or API Key.");
						message.StatusCode = HttpStatusCode.BadRequest;
						message.ReasonPhrase = "Please provide a Truck Number and Api Key";
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error: " + ex.InnerException + "Message: " + ex.Message + "StackTrace: " + ex.StackTrace);
				message.StatusCode = HttpStatusCode.InternalServerError;
				message.ReasonPhrase = "The registration request is either invalid or the system encountered an error when processing it";
			}
			return message;
		}
        #endregion

        #region PostClockIn
        [HttpPost]
		public IHttpActionResult PostClockIn([FromBody] ClockInDto clockIn)
		{
			Log.Info($"Clock In for job {clockIn.JobId.ToString()} at {clockIn.ActualClockIn.ToString()}");

			var job = db.Jobs.Include("TripRoute.Trip").FirstOrDefault(j => j.JobId.Equals(clockIn.JobId));
			if (VerifyBasicAuthCredentials(job.ActualTruckId.Value))
			{
				if (job != null
				&& job.TripRoute.TripStatusId >= Enums.TripStatusEnum.Dispatched.GetHashCode()
				&& job.TripRoute.TripStatusId < Enums.TripStatusEnum.Completed.GetHashCode()
				&& job.TripStatusId == Enums.TripStatusEnum.Dispatched.GetHashCode())
				{
						try
						{
							job.ActualClockIn = clockIn.ActualClockIn;
							job.ActualDriverVendorWorkerId = job.TripRoute.Trip.DriverVendorWorkerId;
							job.TripRoute.TripStatusId = Enums.TripStatusEnum.InProcess.GetHashCode();
							job.TripStatusId = Enums.TripStatusEnum.InProcess.GetHashCode();

							if (job.JobRequiresWeighInOut)
							{
								job.ActualWeightIn = clockIn.ActualWeightIn;
							}

							db.SaveChanges();
							return Ok(clockIn);
						}
						catch (Exception ex)
						{
							Log.Error("Error: " + ex.InnerException + "Message: " + ex.Message + "StackTrace: " + ex.StackTrace);
							return NotFound();
						}
				}
			}
			else
			{
				return Unauthorized();
			}
			return NotFound();
		}
        #endregion

        #region PostClockOut
        [HttpPost]
		public IHttpActionResult PostClockOut([FromBody] ClockOutDto clockOut)
		{
			Log.Info($"Clock Out for job {clockOut.JobId.ToString()} at {clockOut.ActualClockOut.ToString()}, service: {clockOut.ActualServicePerformed}, weight: {(clockOut.ActualWeightOut.HasValue? clockOut.ActualWeightOut.Value.ToString(): "null")}");
			Job job = db.Jobs.Include("TripRoute.Trip").FirstOrDefault(j => j.JobId.Equals(clockOut.JobId));

			if (VerifyBasicAuthCredentials(job.ActualTruckId.Value))
			{
				if (job != null
				&& job.TripRoute.TripStatusId == Enums.TripStatusEnum.InProcess.GetHashCode()
				&& job.TripStatusId == Enums.TripStatusEnum.InProcess.GetHashCode())
				{
					try
					{
						job.ActualClockOut = clockOut.ActualClockOut;
						if (clockOut.ActualServicePerformed.ToLower() == job.JobServiceName.ToLower())
						{
							job.ActualPropertyFlatRateId = job.JobPropertyFlatRateId;
							job.TripStatusId = Enums.TripStatusEnum.Completed.GetHashCode();
						}
						else if (!clockOut.ActualServicePerformed.ToLower().StartsWith("skipped"))
						{
							var propertyFlatRateContract = db.PropertyFlatRates.FirstOrDefault(p => p.PropertyFlatRateId.Equals(job.JobPropertyFlatRateId));
							job.ActualPropertyFlatRateId = LookupAltService(propertyFlatRateContract, clockOut.ActualServicePerformed);
							
							if (!job.ActualPropertyFlatRateId.HasValue)
							{
								job.IsFlaggedForReview = true;
								job.Comments += $" Actual Service performed was {clockOut.ActualServicePerformed}. Cannot determine the matching Flat Rate.";
							}
							else
							{
								job.IsFlaggedForReview = true;
								job.Comments += $" Actual service {clockOut.ActualServicePerformed} is different than requested service {job.JobServiceName}.";
							}
						}
						else if (clockOut.ActualServicePerformed == Enums.TripStatusEnum.SkippedNotPlowed.ToString())
						{
							job.TripStatusId = Enums.TripStatusEnum.SkippedNotPlowed.GetHashCode();
						}
						else if (clockOut.ActualServicePerformed == Enums.TripStatusEnum.SkippedNoAccess.ToString())
						{
							job.TripStatusId = Enums.TripStatusEnum.SkippedNoAccess.GetHashCode();
						}

						if (job.JobRequiresWeighInOut)
						{
							job.ActualWeightOut = clockOut.ActualWeightOut;
							bool compliant = (job.ActualWeightOut.HasValue && job.ActualWeightIn.HasValue);
							job.ActualTonnage =  compliant? job.ActualWeightOut - job.ActualWeightIn : null;
							if (!compliant)
							{
								job.IsFlaggedForReview = true;
								job.Comments += $" Property requires Weigh In and Out but one or both were not provided.";
							}
						}
						job.TripRoute.CompletedStops++;
						if (job.TripRoute.CompletedStops == job.TripRoute.ScheduledStops)
							job.TripRoute.TripStatusId = Enums.TripStatusEnum.Completed.GetHashCode();
						db.SaveChanges();
						return Ok(clockOut);
					}
					catch (Exception ex)
					{
						Log.Error("Error: " + ex.InnerException + "Message: " + ex.Message + "StackTrace: " + ex.StackTrace);
						return NotFound();
					}
				}
				else
				{
					return Unauthorized();
				}
			}			
			return NotFound();
		}
        #endregion

        #region LookupAltService
        /// <summary>
        /// When the actual service performed was not the one requested for the job given by the entity propertyFlatRate, there should be only one alternative.
        /// For example, suppose a property contract has 3 pair of flat lot salt rates, such as "Standard Automatic", "Full Automatic", "Standard Customer Call", "Full Customer Call"
        /// and "Standard Per Ton", "Full Per Ton". Since the mobile app is given the chosen JobPropertyFlatRateId and its Short Name, the alternate service is the one with the same 
        /// qualifying verbiage in its description, but with "Standard" exchanged with "Full" or vice versa. Of course, there are effective and expiration dates to consider...
        /// </summary>
        /// <param name="propertyFlatRate">the requested service to be performed for the job</param>
        /// <param name="actualServiceName">the actual short name of the service performed</param>
        /// <returns>the alternate service</returns>
        private int? LookupAltService(PropertyFlatRate propertyFlatRate, string actualServiceName)
		{
			// just in case the actual service is the same...
			if (actualServiceName.ToLower() == propertyFlatRate.ShortName)
				return propertyFlatRate.PropertyFlatRateId;

			int? altPropertyFlatRateId = null;
			string altServiceName = propertyFlatRate.Description.ToLower().Replace(propertyFlatRate.ShortName.ToLower(), actualServiceName.ToLower());
			// check if the substitution worked.  That is, if the ShortName was contained within the Description the two strings would be different now.
			if (altServiceName != propertyFlatRate.Description.ToLower())
			{
				var now = DateTime.Now;
				// find the alternate service, if it exists...
				var altFlatRates = db.PropertyFlatRates.Where(r => r.PropertyContractId == propertyFlatRate.PropertyContractId
						&& r.ShortName == actualServiceName
						&& r.Description == altServiceName
						&& r.EffectiveDate <= now
						&& (r.ExpirationDate == null || r.ExpirationDate >= now)
						&& r.WorkTypeId == propertyFlatRate.WorkTypeId
						).OrderByDescending(o => (o.ExpirationDate ?? o.EffectiveDate)).ToList();
				if (altFlatRates.Count == 1)
					altPropertyFlatRateId = altFlatRates[0].PropertyFlatRateId;
			}
			return altPropertyFlatRateId;
		}
        #endregion

        #region VerifyBasicAuthCredentials
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
			
			//Add a test...
			//In the below request header add statement change the truck number and apikey (user and password)
			//request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("4210" + ":" + "dev")));  4210 is trucknum and dev is apikey
			
			if (request.Headers.HasKeys() && !String.IsNullOrEmpty(request.Headers.Get("Authorization")))
			{
				string basicAuthHeader = request.Headers.Get("Authorization");
				string basicAuthBase64 = (!String.IsNullOrEmpty(basicAuthHeader) && basicAuthHeader.StartsWith("Basic ")) ? basicAuthHeader.Replace("Basic ", "") : String.Empty;
				if (!string.IsNullOrEmpty(basicAuthBase64))
				{
					try
					{
						var basicAuthBinary = Convert.FromBase64String(basicAuthBase64);
						var basicAuth = ASCIIEncoding.ASCII.GetString(basicAuthBinary);
						var username = $"TRUCK_{basicAuth.Split(':')[0]}";
						var password = basicAuth.Split(':')[1];
						return VerifyAuthentication(username, password, truckId);
					}
					catch (Exception ex)
					{
						string message = ex.Message;
						Log.Error($"Basic Authentication header from truck ID {truckId} is bad. Error: {message}");
					}
				}
				else
					Log.Error($"Authorization header value is missing or not supported for truck ID {truckId}");
			}
			Log.Error($"Authorization header is missing for truck ID {truckId}");
			return result;
		}
        #endregion

        #region VerifyAuthentication
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
		#endregion
	}
}
