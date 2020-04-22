using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaskTrip.BusinessObjects.Models
{
	public class TruckRegistration
	{
		public int TruckId { get; set; } //user (4 digit int)
		public string ApiKey { get; set; } //pw 
		public int DriverId { get; set; } 
	}
}