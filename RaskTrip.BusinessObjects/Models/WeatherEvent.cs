using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class WeatherEvent
	{
		public int WeatherEventId { get; set; }
		public int SeasonId { get; set; }
		public int VendorId { get; set; }
		public string Name { get; set; }
		public int Temperature { get; set; }
		public int AccumulationTypeId { get; set; }
		public int WindTypeId { get; set; }
		public bool IsDrifting { get; set; }
		public System.DateTime StartDate { get; set; }
		public Nullable<System.DateTime> EndDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }
		public Nullable<int> TerritoryId { get; set; }

		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Trip> Trips { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<TripRoute> TripRoutes { get; set; }
		public virtual Vendor Vendor { get; set; }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<WorkEvent> WorkEvents { get; set; }
	}
}
