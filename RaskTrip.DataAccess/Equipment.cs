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
    
    public partial class Equipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipment()
        {
            this.GeneralTimes = new HashSet<GeneralTime>();
            this.LotSaltTimes = new HashSet<LotSaltTime>();
            this.PropertyFeeRates = new HashSet<PropertyFeeRate>();
            this.ProposalFeeRates = new HashSet<ProposalFeeRate>();
            this.PushTimeTickets = new HashSet<PushTimeTicket>();
            this.SidewalkTimeEquipments = new HashSet<SidewalkTimeEquipment>();
            this.Trucks = new HashSet<Truck>();
            this.VendorEquipments = new HashSet<VendorEquipment>();
            this.VendorFeeRates = new HashSet<VendorFeeRate>();
        }
    
        public int EquipmentId { get; set; }
        public int WorkTypeId { get; set; }
        public string Name { get; set; }
        public Nullable<int> EquipmentOwnerTypeId { get; set; }
        public Nullable<int> EquipmentCategoryId { get; set; }
        public Nullable<double> DefaultCustomerRate { get; set; }
        public Nullable<double> DefaultVendorRate { get; set; }
        public string QbItemName { get; set; }
        public string QbReference { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual EquipmentCategory EquipmentCategory { get; set; }
        public virtual EquipmentOwnerType EquipmentOwnerType { get; set; }
        public virtual WorkType WorkType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GeneralTime> GeneralTimes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LotSaltTime> LotSaltTimes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertyFeeRate> PropertyFeeRates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProposalFeeRate> ProposalFeeRates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PushTimeTicket> PushTimeTickets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SidewalkTimeEquipment> SidewalkTimeEquipments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Truck> Trucks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorEquipment> VendorEquipments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VendorFeeRate> VendorFeeRates { get; set; }
    }
}
