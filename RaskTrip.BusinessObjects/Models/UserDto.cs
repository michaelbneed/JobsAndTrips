using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class UserDto
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string EmailAddress { get; set; }
		public string PasswordQuestion { get; set; }
		public string PasswordAnswer { get; set; }
		public bool IsLockedOut { get; set; }
		public Nullable<System.DateTime> LastLoginDate { get; set; }
		public Nullable<System.DateTime> LastPasswordChangeDate { get; set; }
		public int FailedPasswordAttemptCount { get; set; }
		public bool MustChangePassword { get; set; }
		public bool IsActive { get; set; }
		public Nullable<int> VendorId { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }
		public string MobilePhone { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<JobDto> Jobs1 { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<TruckDto> Trucks { get; set; }
		
		public virtual VendorDto Vendor { get; set; }
	}
}
