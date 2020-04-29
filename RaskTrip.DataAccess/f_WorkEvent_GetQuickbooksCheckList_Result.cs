//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RaskTrip.DataAccess
{
    using System;
    
    public partial class f_WorkEvent_GetQuickbooksCheckList_Result
    {
        public long WorkEventId { get; set; }
        public System.DateTime ServiceDate { get; set; }
        public Nullable<int> WeatherEventId { get; set; }
        public int WorkTypeId { get; set; }
        public int ForemanId { get; set; }
        public int VendorId { get; set; }
        public int PropertyId { get; set; }
        public bool IsFuelSurcharge { get; set; }
        public Nullable<int> EquipmentId { get; set; }
        public long Id { get; set; }
        public int VendorPropertyId { get; set; }
        public Nullable<System.DateTime> StartTime1 { get; set; }
        public Nullable<System.DateTime> EndTime1 { get; set; }
        public Nullable<int> StartTime2 { get; set; }
        public Nullable<int> EndTime2 { get; set; }
        public int IsHourlyRate { get; set; }
        public double FlatRate { get; set; }
        public double HourlyRate { get; set; }
        public string EquipmentName { get; set; }
        public Nullable<int> PropertyFlatRateId { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public int TotalAmountWithFuelSurcharge { get; set; }
        public Nullable<int> VendorWorkerId { get; set; }
        public Nullable<int> JobCompleteVendorId { get; set; }
        public Nullable<int> TotalWorkers { get; set; }
        public string EquipmentQbReference { get; set; }
        public string EquipmentQbItemName { get; set; }
        public string ReferenceNumber { get; set; }
        public Nullable<int> RouteInfo { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string StateAbbr { get; set; }
        public string ZipCode { get; set; }
        public string CheckName { get; set; }
        public string PropertyName { get; set; }
        public string LineItemEquipmentName { get; set; }
        public string BusinessName { get; set; }
        public string VendorQbReference { get; set; }
        public string VendorIssues { get; set; }
        public string JobWorkerName { get; set; }
        public string PropertyQbReference { get; set; }
        public string EquipmentQbItemNameDisplay { get; set; }
        public string CompanyName { get; set; }
        public string CompanyQbReference { get; set; }
        public string PropertyQbJob { get; set; }
        public string WeatherEventName { get; set; }
        public Nullable<int> WorkTypeDisplayOrder { get; set; }
        public int IsWce1OnFile { get; set; }
    }
}