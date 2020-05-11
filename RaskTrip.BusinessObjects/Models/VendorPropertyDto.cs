using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class VendorPropertyDto
	{
		public int VendorPropertyId { get; set; }
		public int VendorId { get; set; }
		public int PropertyId { get; set; }
		public int WorkTypeId { get; set; }
		public Nullable<int> FrequencyTypeId { get; set; }
		public System.DateTime EffectiveDate { get; set; }
		public Nullable<System.DateTime> ExpirationDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		public virtual FrequencyTypeDto FrequencyType { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LotSaltTimeDto> LotSaltTimes { get; set; }
		public virtual PropertyDto Property { get; set; }
		
		public virtual VendorDto Vendor { get; set; }
		public virtual WorkTypeDto WorkType { get; set; }
	}
}
