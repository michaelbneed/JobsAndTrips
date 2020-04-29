using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class Vendor
	{
		public int VendorId { get; set; }
		public string BusinessName { get; set; }
		public string CheckName { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public Nullable<int> StateId { get; set; }
		public string ZipCode { get; set; }
		public string AttentionName { get; set; }
		public string EinSsn { get; set; }
		public bool IsContactSheetOnfile { get; set; }
		public bool IsInsuranceOnFile { get; set; }
		public bool IsWce1OnFile { get; set; }
		public bool IsActive { get; set; }
		public bool IsObsolete { get; set; }
		public bool Is1099 { get; set; }
		public bool IsForeman { get; set; }
		public bool IsOutlying { get; set; }
		public bool IsFuelSurcharge { get; set; }
		public Nullable<System.DateTime> InsuranceExpirationDate { get; set; }
		public Nullable<System.DateTime> Wce1ExpirationDate { get; set; }
		public string ContactName { get; set; }
		public string HomePhoneNumber { get; set; }
		public string WorkPhoneNumber { get; set; }
		public string MobilePhoneNumber { get; set; }
		public string EmergencyPhoneNumber { get; set; }
		public string FaxNumber { get; set; }
		public string EmailAddress { get; set; }
		public string AlternateContactName { get; set; }
		public string AlternateContactPhoneNumber { get; set; }
		public string Trade { get; set; }
		public string QbReference { get; set; }
		public string Comments { get; set; }
		public bool IsSyncNeeded { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }
		public string ABList { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Truck> Trucks { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<User> Users { get; set; }
		
		public virtual ICollection<VendorProperty> VendorProperties { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<VendorRoute> VendorRoutes { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<VendorRoute> VendorRoutes1 { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<VendorWorker> VendorWorkers { get; set; }
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<WeatherEvent> WeatherEvents { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<WorkEvent> WorkEvents { get; set; }
	}
}
