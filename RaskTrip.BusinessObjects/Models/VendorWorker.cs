using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class VendorWorker
	{
		public int VendorWorkerId { get; set; }
		public int VendorId { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Job> Jobs { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LotSaltTime> LotSaltTimes { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Trip> Trips { get; set; }
		public virtual Vendor Vendor { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<VendorRoute> VendorRoutes { get; set; }
	}
}
