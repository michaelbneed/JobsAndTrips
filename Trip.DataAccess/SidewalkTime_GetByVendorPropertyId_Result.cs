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
    
    public partial class SidewalkTime_GetByVendorPropertyId_Result
    {
        public long SidewalkTimeId { get; set; }
        public long WorkEventId { get; set; }
        public int VendorPropertyId { get; set; }
        public System.DateTime StartTime { get; set; }
        public double HoursPerPerson { get; set; }
        public int TotalWorkers { get; set; }
        public bool IsCustomerSupplied { get; set; }
        public string Notes { get; set; }
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
