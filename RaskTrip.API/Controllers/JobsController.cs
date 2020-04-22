using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RaskTrip.BusinessObjects;
using RaskTrip.BusinessObjects.Models;


namespace RaskTrip.API.Controllers
{
	public class JobsController : ApiController
	{
		//public Job Get(int truckId)
		//{
		//	var job = new Job();

		//	job.PropertyInfo = new PropertyInfo();
		//	job.ServiceInfo = new ServiceInfo();

		//	job.ServiceInfo.AcctManager = new AccountManager();
		//	job.ServiceInfo.OpsContact = new OperationsContact();

		//	ServiceInfo[] altServiceOptions = null;

		//	// Get job by truckid - first or default
		//	// job = dbcontext.Jobs.FirstOrDefault(j => j.TruckId.Equals(truckId));

		//	// mock a job and populate child classes
		//	// Move out of here upon Iface extraction, into a test suite
		//	job.JobId = 1;

		//	job.PropertyInfo.CoordsLongitude = 39.918570;
		//	job.PropertyInfo.CoordsLatitude = -85.985832;
		//	job.PropertyInfo.ArrivalRadius = 1;
		//	job.PropertyInfo.PropertyAddress = "31 E MAIN ST CARMEL IN 46032";
		//	job.PropertyInfo.PropertyId = 1;
		//	job.PropertyInfo.PropertyName = "Agave Bar and Grill";
		//	job.PropertyInfo.SiteFotoUrl = "https://www.sitefotos.com/vpics/guestmapdev?y3v7h0";

		//	job.ServiceInfo.ServiceId = 1;
		//	job.ServiceInfo.SpecialInstructions = "Some special stuff is special!";
		//	job.ServiceInfo.ServiceName = "Hey, NICE service?";
		//	job.ServiceInfo.AltServiceOptions = altServiceOptions;

		//	job.ServiceInfo.OpsContact.ContactName = "Jim Dee";
		//	job.ServiceInfo.OpsContact.ContactPhone = "222.333.4444";

		//	job.ServiceInfo.AcctManager.ManagerName = "John Doe";
		//	job.ServiceInfo.AcctManager.ManagerPhone = "123-123-1234";
		//	return job;
		//}



		//[HttpGet]
		public IHttpActionResult GetNextJob()
		{
			//var job = dbContext.FirstOrDefault(p => p.Id == truckId).OrderByDesc;
			var job = new Job();

			job.PropertyInfo = new PropertyInfo();
			job.ServiceInfo = new ServiceInfo();

			job.ServiceInfo.AcctManager = new AccountManager();
			job.ServiceInfo.OpsContact = new OperationsContact();

			ServiceInfo[] altServiceOptions = null;

			// Get job by truckid - first or default
			// job = dbcontext.Jobs.FirstOrDefault(j => j.TruckId.Equals(truckId));

			// mock a job and populate child classes
			// Move out of here upon Iface extraction, into a test suite
			job.JobId = 1;

			job.PropertyInfo.CoordsLongitude = 39.918570;
			job.PropertyInfo.CoordsLatitude = -85.985832;
			job.PropertyInfo.ArrivalRadius = 1;
			job.PropertyInfo.PropertyAddress = "31 E MAIN ST CARMEL IN 46032";
			job.PropertyInfo.PropertyId = 1;
			job.PropertyInfo.PropertyName = "Agave Bar and Grill";
			job.PropertyInfo.SiteFotoUrl = "https://www.sitefotos.com/vpics/guestmapdev?y3v7h0";

			job.ServiceInfo.ServiceId = 1;
			job.ServiceInfo.SpecialInstructions = "Some special stuff is special!";
			job.ServiceInfo.ServiceName = "Hey, NICE service?";
			job.ServiceInfo.AltServiceOptions = altServiceOptions;

			job.ServiceInfo.OpsContact.ContactName = "Jim Dee";
			job.ServiceInfo.OpsContact.ContactPhone = "222.333.4444";

			job.ServiceInfo.AcctManager.ManagerName = "John Doe";
			job.ServiceInfo.AcctManager.ManagerPhone = "123-123-1234";

			if (job == null)
			{
				return NotFound();
			}

			return Ok(job);
		}

		[HttpPost]
		public IHttpActionResult PostRegisterTruck(TruckRegistration truckRegistration)
		{
			// EF 6 insert a new truck registration and return status
			// this is an object to return
			return Ok(truckRegistration);
		}

		[HttpPost]
		public void PostClockIn(StringContent jobData)
		{
			var clockInTime = DateTime.Now;
			// Work with team/Dave P on data points
		}

		[HttpPost]
		public void PostClockOut(StringContent jobData)
		{
			var clockOutTime = DateTime.Now;
			// Work with team/Dave P on data points
		}
	}
}
