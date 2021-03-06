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
    
    public partial class PropertyFlatRate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyFlatRate()
        {
            this.Jobs = new HashSet<Job>();
            this.Jobs1 = new HashSet<Job>();
            this.Jobs2 = new HashSet<Job>();
            this.LotSaltTimes = new HashSet<LotSaltTime>();
        }
    
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
        public virtual ICollection<Job> Jobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LotSaltTime> LotSaltTimes { get; set; }
        public virtual PropertyContract PropertyContract { get; set; }
        public virtual WorkType WorkType { get; set; }
    }
}
