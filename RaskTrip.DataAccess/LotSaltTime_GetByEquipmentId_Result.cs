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
    
    public partial class LotSaltTime_GetByEquipmentId_Result
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
    }
}
