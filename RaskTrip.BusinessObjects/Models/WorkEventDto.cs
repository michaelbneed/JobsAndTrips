using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class WorkEventDto
	{
		public long WorkEventId { get; set; }
		public int VendorId { get; set; }
		public System.DateTime ServiceDate { get; set; }
		public int WorkTypeId { get; set; }
		public Nullable<int> WeatherEventId { get; set; }
		public bool IsApproved { get; set; }
		public Nullable<System.DateTime> ApprovedDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LotSaltTimeDto> LotSaltTimes { get; set; }
		
		public virtual VendorDto Vendor { get; set; }
		public virtual WeatherEventDto WeatherEvent { get; set; }
		public virtual WorkTypeDto WorkType { get; set; }
	}
}
