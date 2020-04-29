using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class Equipment
	{
		public int EquipmentId { get; set; }
		public int WorkTypeId { get; set; }
		public string Name { get; set; }
		public Nullable<int> EquipmentOwnerTypeId { get; set; }
		public Nullable<int> EquipmentCategoryId { get; set; }
		public Nullable<double> DefaultCustomerRate { get; set; }
		public Nullable<double> DefaultVendorRate { get; set; }
		public string QbItemName { get; set; }
		public string QbReference { get; set; }
		public bool IsActive { get; set; }
		public Nullable<int> DisplayOrder { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		public virtual WorkType WorkType { get; set; }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LotSaltTime> LotSaltTimes { get; set; }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Truck> Trucks { get; set; }		
	}
}