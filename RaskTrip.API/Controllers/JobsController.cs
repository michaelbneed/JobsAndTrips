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

namespace RaskTrip.API.Controllers
{
	public class JobsController : ApiController
	{
		RaskTrip_Entities db = new RaskTrip_Entities();
		
		[HttpGet]
		public IHttpActionResult GetNextJob(int truckId)
		{
			db.Configuration.ProxyCreationEnabled = false;
			
			var job =  db.Jobs.Include("Property").Include("PropertyAddress").FirstOrDefault(s => s.ActualTruckId == truckId);
			JobDto jobDto = new JobDto();

			jobDto.JobServiceName = job.JobServiceName;
			jobDto.PropertyName = job.PropertyName;
			var propertyContact = db.PropertyContacts.FirstOrDefault(n => n.PropertyId == job.PropertyId);
			jobDto.PropertyContactName = propertyContact.Name;
			jobDto.PropertyContactPhone = propertyContact.WorkPhoneNumber;
			jobDto.JobId = job.JobId;
			jobDto.Street1 = job.PropertyAddress.Street1;
			jobDto.Street2 = job.PropertyAddress.Street2;
			jobDto.City = job.PropertyAddress.City;
			jobDto.State = db.States.FirstOrDefault(s => s.StateId == job.PropertyAddress.StateId).StateName;
			jobDto.ZipCode = job.PropertyAddress.ZipCode;

			if (job == null)
			{
				return NotFound();
			}

			var result = Ok(jobDto);

			return result;
		}

		//[HttpPost]
		//[AllowAnonymous]
		public IHttpActionResult PostRegisterTruck([FromBody]object content)
		{
			Truck truck = new Truck();
			User user = new User();
			var truckDto = JsonConvert.DeserializeObject<TruckDto>(content.ToString());

			try
			{
				//var truckDataConverted = JsonConvert.DeserializeObject<TruckDto>(truckDto.ToString());

				if (truckDto != null)
				{
					if (!String.IsNullOrEmpty(truckDto.TruckNumber))
					{
						user = db.Users.FirstOrDefault(u => u.Username.Equals(truckDto.TruckNumber));

						truck.TruckNumber = truckDto.TruckNumber;
						user.Password = truckDto.ApiKey;
					}
				}
			}
			catch (Exception)
			{
				//Loggerlog(ex);  - put in a helper
				throw;
			}
			
			
			db.Trucks.Add(truck);
			
			db.SaveChanges();
			
			var response = Request.CreateResponse<TruckDto>(HttpStatusCode.Created, truckDto);

			return Ok(response);
		}

		//[HttpPost]
		//public void PostClockIn(StringContent jobData)
		//{
		//	var clockInTime = DateTime.Now;
		//	// Work with team/Dave P on data points
		//}

		//[HttpPost]
		//public void PostClockOut(StringContent jobData)
		//{
		//	var clockOutTime = DateTime.Now;
		//	// Work with team/Dave P on data points
		//}
	}
}
