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
    
    public partial class ProposalContact_GetByProposalContactId_Result
    {
        public int ProposalContactId { get; set; }
        public int ProposalId { get; set; }
        public Nullable<int> PropertyContactId { get; set; }
        public int ContactTypeId { get; set; }
        public string Name { get; set; }
        public string WorkPhoneNumber { get; set; }
        public Nullable<int> WorkPhoneNumberExtension { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
