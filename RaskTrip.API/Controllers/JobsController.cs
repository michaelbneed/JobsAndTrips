using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RaskTrip.DataAccess;
using RaskTrip.BusinessObjects.Models;
using RaskTrip.BusinessObjects.Enums;
using System.Linq;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Net;
using NLog;
using Swashbuckle.Swagger.Annotations;
using System.Web;
using System.Text;
using RaskTrip.Utility.Security;
using NLog.Fluent;
using System.Collections.Generic;
using System.Collections;

namespace RaskTrip.API.Controllers
{
	public class JobsController : ApiController
	{
		RaskTrip_Entities db = new RaskTrip_Entities();
		Enums enums = new Enums();

        #region GetNextJob
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(JobDto))]
		[SwaggerResponse(HttpStatusCode.Unauthorized)]
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
			var job = db.Jobs.Include("TripRoute.Trip").FirstOrDefault(j => j.JobId.Equals(clockIn.JobId));
			Log.Info($"Clock In for job {clockIn.JobId.ToString()} at {clockIn.ActualClockIn.ToString()}");
			if (VerifyBasicAuthCredentials(job.ActualTruckId.Value))
			{
				if (job != null
				&& job.TripRoute.TripStatusId >= Enums.TripStatusEnum.Dispatched.GetHashCode()
				&& job.TripRoute.TripStatusId < Enums.TripStatusEnum.Completed.GetHashCode()
				&& job.TripStatusId == Enums.TripStatusEnum.Dispatched.GetHashCode())
				{
					if (job.JobId == clockIn.JobId)
					{
						try
						{
							job.ActualClockIn = DateTime.Now;
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
			var job = db.Jobs.Include("TripRoute.Trip").FirstOrDefault(j => j.JobId.Equals(clockOut.JobId));

			if (VerifyBasicAuthCredentials(job.ActualTruckId.Value))
			{
				if (job != null
				&& job.TripRoute.TripStatusId == Enums.TripStatusEnum.InProcess.GetHashCode()
				&& job.TripStatusId == Enums.TripStatusEnum.InProcess.GetHashCode())
				{
					if (job.JobId == clockOut.JobId)
					{
						try
						{
							job.ActualClockOut = DateTime.Now;

							if (clockOut.ActualServicePerformed.ToLower() == job.JobServiceName.ToLower())
							{
								job.ActualPropertyFlatRateId = job.JobPropertyFlatRateId;
							}
							else
							{
								var propertyFlatRateContractId = db.PropertyFlatRates.FirstOrDefault(p => p.PropertyFlatRateId.Equals(job.JobPropertyFlatRateId)).PropertyContractId;
								List<PropertyFlatRate> propertyFlatRateList = db.PropertyFlatRates.Where(p => p.PropertyContractId == propertyFlatRateContractId).ToList();
								foreach (var item in propertyFlatRateList)
								{
									if (item.ExpirationDate == null)
									{
										// TODO: Pending call with Daniel on the potential bad data we are looking at in this loop
										// TODO: finish logic of comparing the clockOut dto service with the job.servicename
										switch (item.ShortName)
										{
											case "Standard/Partial":
												job.ActualPropertyFlatRateId = item.PropertyFlatRateId;
												break;
											case "Full":
												job.ActualPropertyFlatRateId = item.PropertyFlatRateId;
												break;
											case "SkippedNotPlowed":
												job.ActualPropertyFlatRateId = item.PropertyFlatRateId;
												break;
											case "SkippedNoAccess":
												job.ActualPropertyFlatRateId = item.PropertyFlatRateId;
												break;
											default:
												job.ActualPropertyFlatRateId = job.JobPropertyFlatRateId;
												break;
										}
										//if (item.ShortName.ToLower().Contains(clockOut.ActualServicePerformed))
										//{
										//	job.ActualPropertyFlatRateId = item.PropertyFlatRateId;
										//}
									}
								}
							}

							// TODO: Update the status like below, but might need to account for a variation as above in the loop
							if (clockOut.ActualServicePerformed == Enums.TripStatusEnum.SkippedNotPlowed.ToString())
							{
								job.TripStatusId = Enums.TripStatusEnum.SkippedNotPlowed.GetHashCode();
							}
							else if (clockOut.ActualServicePerformed == Enums.TripStatusEnum.SkippedNoAccess.ToString())
							{
								job.TripStatusId = Enums.TripStatusEnum.SkippedNoAccess.GetHashCode();
							}
							else if (clockOut.ActualServicePerformed == Enums.TripStatusEnum.Completed.ToString())
							{
								job.TripStatusId = Enums.TripStatusEnum.Completed.GetHashCode();
							}

							if (job.JobRequiresWeighInOut)
							{
								job.ActualWeightOut = clockOut.ActualWeightOut;
								job.ActualTonnage = job.ActualWeightIn = clockOut.ActualWeightOut;
							}

							db.SaveChanges();
							return Ok(clockOut);
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
			}			
			return NotFound();
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
				var basicAuthBinary = Convert.FromBase64String(basicAuthBase64);
				var basicAuth = ASCIIEncoding.ASCII.GetString(basicAuthBinary);
				var username = $"TRUCK_{basicAuth.Split(':')[0]}";
				var password = basicAuth.Split(':')[1];
				return VerifyAuthentication(username, password, truckId);
			}
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
