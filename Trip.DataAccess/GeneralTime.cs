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
    using System.Collections.Generic;
    
    public partial class GeneralTime
    {
        public long GeneralTimeId { get; set; }
        public long WorkEventId { get; set; }
        public int DriverId { get; set; }
        public int EquipmentId { get; set; }
        public System.DateTime StartTime { get; set; }
        public double Hours { get; set; }
        public bool IsCheckRun { get; set; }
        public Nullable<System.DateTime> CheckRunDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    
        public virtual Equipment Equipment { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual WorkEvent WorkEvent { get; set; }
    }
}
