﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class LotSaltTime
	{
		public long LotSaltTimeId { get; set; }
		public long WorkEventId { get; set; }
		public int VendorPropertyId { get; set; }
		public int VendorWorkerId { get; set; }
		public Nullable<int> PropertyFlatRateId { get; set; }
		public Nullable<int> EquipmentId { get; set; }
		public bool IsHourlyRate { get; set; }
		public System.DateTime StartTime { get; set; }
		public Nullable<System.DateTime> EndTime { get; set; }
		public Nullable<double> Tonnage { get; set; }
		public string Comments { get; set; }
		public bool IsInvoiced { get; set; }
		public Nullable<System.DateTime> InvoicedDate { get; set; }
		public bool IsCheckRun { get; set; }
		public Nullable<System.DateTime> CheckRunDate { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		public virtual Equipment Equipment { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs { get; set; }
		public virtual PropertyFlatRate PropertyFlatRate { get; set; }
		public virtual VendorProperty VendorProperty { get; set; }
		public virtual VendorWorker VendorWorker { get; set; }
		public virtual WorkEvent WorkEvent { get; set; }
	}
}
