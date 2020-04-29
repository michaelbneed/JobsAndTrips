using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaskTrip.BusinessObjects.Models
{
	public class Truck
	{
		public int TruckId { get; set; }
		public string TruckNumber { get; set; }
		public int VendorId { get; set; }
		public Nullable<int> EquipmentId { get; set; }
		public Nullable<int> TripApiUserId { get; set; }
		public string MobilePhone { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		public virtual Equipment Equipment { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Trip> Trips { get; set; }
		public virtual User User { get; set; }
		public virtual Vendor Vendor { get; set; }
	}
}