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
    
    public partial class CompanyAddress_GetByAddressTypeId_Result
    {
        public int CompanyAddressId { get; set; }
        public int CompanyId { get; set; }
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
    }
}