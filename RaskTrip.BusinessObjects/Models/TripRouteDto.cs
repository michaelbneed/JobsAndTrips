using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class TripRouteDto
	{
		public int TripRouteId { get; set; }
		public int WeatherEventId { get; set; }
		public int VendorRouteId { get; set; }
		public string RouteName { get; set; }
		public Nullable<int> FilterByFrequencyTypeId { get; set; }
		public Nullable<int> TripId { get; set; }
		public Nullable<int> TripRouteOrder { get; set; }
		public string SplitRange { get; set; }
		public int TotalStops { get; set; }
		public int ScheduledStops { get; set; }
		public int CompletedStops { get; set; }
		public bool IsPublished { get; set; }
		public int TripStatusId { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		public virtual TripDto Trip { get; set; }
		public virtual TripStatusDto TripStatu { get; set; }
	}
}
