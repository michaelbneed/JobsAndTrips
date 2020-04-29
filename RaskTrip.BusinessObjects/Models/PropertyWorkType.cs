using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
	public class PropertyWorkType
	{
		public int PropertyWorkTypeId { get; set; }
		public int PropertyId { get; set; }
		public int WorkTypeId { get; set; }
		public string Notes { get; set; }
		public string CreatedBy { get; set; }
		public System.DateTime CreatedOn { get; set; }
		public string UpdatedBy { get; set; }
		public Nullable<System.DateTime> UpdatedOn { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Job> Jobs { get; set; }
		public virtual Property Property { get; set; }
		public virtual WorkType WorkType { get; set; }
	}
}
