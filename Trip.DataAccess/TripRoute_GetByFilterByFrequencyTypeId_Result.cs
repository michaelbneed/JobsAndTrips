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
    
    public partial class TripRoute_GetByFilterByFrequencyTypeId_Result
    {
        public int TripRouteId { get; set; }
        public int WeatherEventId { get; set; }
        public int VendorRouteId { get; set; }
        public string RouteName { get; set; }
        public Nullable<int> FilterByFrequencyTypeId { get; set; }
        public Nullable<int> TripId { get; set; }
        public Nullable<int> TripRouteOrder { get; set; }
        public string SplitRange { get; set; }
        public int TotalStops { get; set; }
        public int ScheduledStops { get; set; }
        public int CompletedStops { get; set; }
        public bool IsPublished { get; set; }
        public int TripStatusId { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
