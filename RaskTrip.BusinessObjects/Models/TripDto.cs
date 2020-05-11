using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{	
	public class TripDto
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

		public virtual TruckDto Truck { get; set; }
		public virtual VendorWorkerDto VendorWorker { get; set; }
		public virtual WeatherEventDto WeatherEvent { get; set; }
		public virtual WorkTypeDto WorkType { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<TripRouteDto> TripRoutes { get; set; }
	}
}
