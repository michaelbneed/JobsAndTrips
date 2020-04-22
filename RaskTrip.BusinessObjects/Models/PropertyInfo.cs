using System;
using System.Collections.Generic;
using System.Linq;

namespace RaskTrip.BusinessObjects.Models
{
	public class PropertyInfo
	{
		public int PropertyId { get; set; }
		public string PropertyName { get; set; }
		public string PropertyAddress { get; set; }
		public double CoordsLongitude { get; set; }
		public double CoordsLatitude { get; set; }
		public double ArrivalRadius { get; set; }
		public string SiteFotoUrl { get; set; }
	}
}
