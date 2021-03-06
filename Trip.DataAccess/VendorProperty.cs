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
    
    public partial class VendorProperty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VendorProperty()
        {
            this.Jobs = new HashSet<Job>();
            this.LotSaltTimes = new HashSet<LotSaltTime>();
            this.PushTimeTicketProperties = new HashSet<PushTimeTicketProperty>();
            this.SidewalkTimes = new HashSet<SidewalkTime>();
            this.TimeTicketProperties = new HashSet<TimeTicketProperty>();
            this.VendorPropertyRoutes = new HashSet<VendorPropertyRoute>();
        }
    
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
    
        public virtual FrequencyType FrequencyType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LotSaltTime> LotSaltTimes { get; set; }
        public virtual Property Property { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PushTimeTicketProperty> PushTimeTicketProperties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SidewalkTime> SidewalkTimes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeTicketProperty> TimeTicketProperties { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual WorkType WorkType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorPropertyRoute> VendorPropertyRoutes { get; set; }
    }
}
