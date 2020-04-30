using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using RaskTrip.BusinessObjects;
using RaskTrip.DataAccess;
using System.Linq;

namespace RaskTrip.API.Controllers
{
	public class JobsController : ApiController
	{
		RaskTrip_Entities db = new RaskTrip_Entities();

		//[System.Web.Http.Description.ResponseType(typeof(Job))]
		public IHttpActionResult GetNextJob(int truckId)
		{
			db.Configuration.ProxyCreationEnabled = false;

			var job = db.Jobs.FirstOrDefault(s => s.ActualTruckId == truckId);
			var jobId = job.JobId;

			if (job == null)
			{
				return NotFound();
			}

			var result = Ok(job);
			return result;
		}

		[HttpPost]
		public IHttpActionResult PostRegisterTruck(StringContent truckRegistration)
		{
			// EF 6 insert a new truck registration and return status
			// this is an object to return
			return Ok(truckRegistration);
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
