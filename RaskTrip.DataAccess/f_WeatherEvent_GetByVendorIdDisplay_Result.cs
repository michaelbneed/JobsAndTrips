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
    
    public partial class f_WeatherEvent_GetByVendorIdDisplay_Result
    {
        public int WeatherEventId { get; set; }
        public int SeasonId { get; set; }
        public int VendorId { get; set; }
        public string Name { get; set; }
        public int Temperature { get; set; }
        public int AccumulationTypeId { get; set; }
        public int WindTypeId { get; set; }
        public bool IsDrifting { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> TerritoryId { get; set; }
        public string AccumulationTypeName { get; set; }
        public string WindTypeName { get; set; }
        public string SurfaceTypeName { get; set; }
        public string PrecipitationTypeName { get; set; }
    }
}
