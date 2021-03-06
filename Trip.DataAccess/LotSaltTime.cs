//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trip.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class LotSaltTime
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LotSaltTime()
        {
            this.Jobs = new HashSet<Job>();
        }
    
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
        public virtual ICollection<Job> Jobs { get; set; }
        public virtual PropertyFlatRate PropertyFlatRate { get; set; }
        public virtual VendorProperty VendorProperty { get; set; }
        public virtual VendorWorker VendorWorker { get; set; }
        public virtual WorkEvent WorkEvent { get; set; }
    }
}
