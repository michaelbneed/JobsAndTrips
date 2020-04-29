using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{	
	public class Trip
		{
		public int TripId { get; set; }
		public int WeatherEventId { get; set; }
		public int WorkTypeId { get; set; }
		public int TruckId { get; set; }
		public Nullable<int> DriverVendorWorkerId { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		public virtual Truck Truck { get; set; }
		public virtual VendorWorker VendorWorker { get; set; }
		public virtual WeatherEvent WeatherEvent { get; set; }
		public virtual WorkType WorkType { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<TripRoute> TripRoutes { get; set; }
	}
}
