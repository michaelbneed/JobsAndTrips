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
    
    public partial class Job_Get_List_Result
    {
        public long JobId { get; set; }
        public long WorkEventId { get; set; }
        public int TripRouteId { get; set; }
        public int VendorRouteId { get; set; }
        public int VendorPropertyId { get; set; }
        public int StopOrder { get; set; }
        public int JobStopOrder { get; set; }
        public bool IsSelectedByFilter { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsPublished { get; set; }
        public bool IsHourly { get; set; }
        public bool IsFlaggedForReview { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public int PropertyAddressId { get; set; }
        public Nullable<int> SalesRepUserId { get; set; }
        public Nullable<int> OperationsUserId { get; set; }
        public Nullable<int> PropertyWorkTypeId { get; set; }
        public int DefaultPropertyFlatRateId { get; set; }
        public int JobPropertyFlatRateId { get; set; }
        public bool JobIsTonnage { get; set; }
        public Nullable<double> JobFixedTonnage { get; set; }
        public string JobServiceName { get; set; }
        public bool JobRequiresWeighInOut { get; set; }
        public Nullable<long> LotSaltTimeId { get; set; }
        public Nullable<int> ActualDriverVendorWorkerId { get; set; }
        public Nullable<int> ActualTruckId { get; set; }
        public Nullable<System.DateTime> ActualClockIn { get; set; }
        public Nullable<System.DateTime> ActualClockOut { get; set; }
        public Nullable<double> ActualWeightIn { get; set; }
        public Nullable<double> ActualWeightOut { get; set; }
        public Nullable<int> ActualPropertyFlatRateId { get; set; }
        public Nullable<double> ActualTonnage { get; set; }
        public int TripStatusId { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
