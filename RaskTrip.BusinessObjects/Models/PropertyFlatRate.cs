using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class PropertyFlatRate
	{
		public int PropertyFlatRateId { get; set; }
		public int PropertyContractId { get; set; }
		public int WorkTypeId { get; set; }
		public string Description { get; set; }
		public Nullable<double> MinValue { get; set; }
		public Nullable<double> MaxValue { get; set; }
		public double Rate { get; set; }
		public double VendorFee { get; set; }
		public bool IsTonnage { get; set; }
		public bool IsDefault { get; set; }
		public System.DateTime EffectiveDate { get; set; }
		public Nullable<System.DateTime> ExpirationDate { get; set; }
		public Nullable<int> DisplayOrder { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }
		public string ShortName { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs1 { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs2 { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LotSaltTime> LotSaltTimes { get; set; }
		
		public virtual WorkType WorkType { get; set; }
	}
}
