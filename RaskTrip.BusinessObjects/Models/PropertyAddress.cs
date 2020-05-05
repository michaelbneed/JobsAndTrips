﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class PropertyAddress
	{
		public int PropertyAddressId { get; set; }
		public int PropertyId { get; set; }
		public int AddressTypeId { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public Nullable<int> StateId { get; set; }
		public string ZipCode { get; set; }
		public string Attention { get; set; }
		public bool IsSyncNeeded { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }
		public Nullable<double> GpsLatitude { get; set; }
		public Nullable<double> GpsLongitude { get; set; }
		public Nullable<double> GpsRadius { get; set; }

		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs { get; set; }
		public virtual PropertyDto Property { get; set; }
		
	}
}
