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
    
    public partial class f_VendorProperty_GetByPropertyIdDisplay_Result
    {
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
        public string WorkTypeName { get; set; }
        public string BusinessName { get; set; }
        public string FrequencyTypeName { get; set; }
    }
}
