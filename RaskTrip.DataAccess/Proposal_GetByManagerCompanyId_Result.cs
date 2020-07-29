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
    
    public partial class Proposal_GetByManagerCompanyId_Result
    {
        public int ProposalId { get; set; }
        public int ProposalStatusId { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string Landmark { get; set; }
        public Nullable<int> TerritoryId { get; set; }
        public Nullable<int> ManagerCompanyId { get; set; }
        public string ManagerName { get; set; }
        public Nullable<int> OwnerCompanyId { get; set; }
        public string OwnerName { get; set; }
        public Nullable<double> LotSizeAcres { get; set; }
        public Nullable<double> SidewalkInHours { get; set; }
        public Nullable<int> SidewalkSqFt { get; set; }
        public Nullable<double> PlowtimeInHours { get; set; }
        public Nullable<double> IcemeltBags { get; set; }
        public string SalesRep { get; set; }
        public string BillingEmailAddress { get; set; }
        public bool IsFuelSurcharge { get; set; }
        public bool IsFinanceCharge { get; set; }
        public Nullable<int> PaymentTermId { get; set; }
        public string PoNumber { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
