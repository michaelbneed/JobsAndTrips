using System;
using System.Collections.Generic;
using System.Linq;

namespace RaskTrip.BusinessObjects.Models
{

	public class Service
	{
		public int ServiceId { get; set; }
		public string ServiceName { get; set; }
		public string SpecialInstructions { get; set; }
		public string SiteFotoUrl { get; set; }
		
		public OperationsContact OpsContact { get; set; }
		public Service[] AltServiceOptions { get; set; }
	}

}
