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
    
    public partial class ProposalFlatRate_GetByProposalId_Result
    {
        public int ProposalFlatRateId { get; set; }
        public int ProposalId { get; set; }
        public int WorkTypeId { get; set; }
        public string Description { get; set; }
        public Nullable<double> MinValue { get; set; }
        public Nullable<double> MaxValue { get; set; }
        public double Rate { get; set; }
        public double VendorFee { get; set; }
        public Nullable<bool> IsTonnage { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
