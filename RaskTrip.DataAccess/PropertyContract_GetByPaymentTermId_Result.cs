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
    
    public partial class PropertyContract_GetByPaymentTermId_Result
    {
        public int PropertyContractId { get; set; }
        public int PropertyId { get; set; }
        public bool IsSignatureRequired { get; set; }
        public bool IsFuelSurcharge { get; set; }
        public bool IsFinanceCharge { get; set; }
        public bool IsCreditCardFee { get; set; }
        public bool IsThirdParty { get; set; }
        public Nullable<int> PaymentTermId { get; set; }
        public string PoNumber { get; set; }
        public bool IsProposal { get; set; }
        public System.DateTime EffectiveDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool IsVerbal { get; set; }
    }
}
