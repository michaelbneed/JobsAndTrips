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
    
    public partial class VendorEquipment_GetByForemanId_Result
    {
        public int VendorEquipmentId { get; set; }
        public int VendorId { get; set; }
        public Nullable<int> ForemanId { get; set; }
        public int EquipmentId { get; set; }
        public Nullable<int> Number { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> TerritoryId { get; set; }
    }
}
