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
    
    public partial class f_Vendor_FeeRateReport_Result
    {
        public int VendorId { get; set; }
        public string BusinessName { get; set; }
        public string ContactName { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string ForemanName { get; set; }
        public string ForemanMobilePhoneNumber { get; set; }
        public string EquipmentName { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<int> Number { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string IsActive { get; set; }
        public string TerritoryName { get; set; }
    }
}
