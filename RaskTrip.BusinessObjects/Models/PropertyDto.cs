using System;
using System.Collections.Generic;
using System.Linq;

namespace RaskTrip.BusinessObjects.Models
{
	public class PropertyDto
	{
		public int PropertyId { get; set; }
		public string PropertyName { get; set; }
		public string Landmark { get; set; }
		public string OnsiteManager { get; set; }
		public string OnsitePhone { get; set; }
		public Nullable<int> TerritoryId { get; set; }
		public Nullable<double> LotSizeAcres { get; set; }
		public Nullable<double> SidewalkInHours { get; set; }
		public Nullable<int> SidewalkSqFt { get; set; }
		public Nullable<double> PlowtimeInHours { get; set; }
		public Nullable<double> IcemeltBags { get; set; }
		public string SalesRep { get; set; }
		public string BillingEmailAddress { get; set; }
		public string QbJob { get; set; }
		public string QbReference { get; set; }
		public string Comments { get; set; }
		public bool IsActiveContract { get; set; }
		public bool IsSyncNeeded { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }
		public string HoursOfOperation { get; set; }
		public string SitefotosUrl { get; set; }
		public bool Open24Hours { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs { get; set; }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<PropertyAddressDto> PropertyAddresses { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<PropertyContactDto> PropertyContacts { get; set; }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<PropertyWorkTypeDto> PropertyWorkTypes { get; set; }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<VendorPropertyDto> VendorProperties { get; set; }
	}
}
