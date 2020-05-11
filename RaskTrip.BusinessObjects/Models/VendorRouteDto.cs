using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class VendorRouteDto
	{
		public int VendorRouteId { get; set; }
		public int VendorId { get; set; }
		public string RouteName { get; set; }
		public Nullable<int> AssignedVendorId { get; set; }
		public Nullable<int> VendorWorkerId { get; set; }
		public System.DateTime EffectiveDate { get; set; }
		public Nullable<System.DateTime> ExpirationDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }
		public Nullable<int> TerritoryId { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs { get; set; }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<TripRouteDto> TripRoutes { get; set; }
		public virtual VendorDto Vendor { get; set; }
		public virtual VendorDto Vendor1 { get; set; }
		
		public virtual VendorWorkerDto VendorWorker { get; set; }
	}
}
