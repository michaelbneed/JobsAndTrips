using System;
using System.Collections.Generic;
using System.Linq;

namespace RaskTrip.BusinessObjects.Models
{
	public class JobDto
	{
		public long JobId { get; set; }
		public string JobServiceName { get; set; }

		public string PropertyName { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string PropertyContactName { get; set; }
		public string PropertyContactPhone { get; set; }

		public Nullable<int> ActualTruckId { get; set; }
		public string TruckNumber { get; set; }
		public Nullable<int> TripApiUserId { get; set; }



		//public long WorkEventId { get; set; }
		//public int TripRouteId { get; set; }
		//public int VendorRouteId { get; set; }
		//public int VendorPropertyId { get; set; }
		//public int StopOrder { get; set; }
		//public int JobStopOrder { get; set; }
		//public bool IsSelectedByFilter { get; set; }
		//public bool IsScheduled { get; set; }
		//public bool IsPublished { get; set; }
		//public bool IsHourly { get; set; }
		//public bool IsFlaggedForReview { get; set; }
		//public int PropertyId { get; set; }
		//public string PropertyName { get; set; }
		//public int PropertyAddressId { get; set; }
		//public Nullable<int> SalesRepUserId { get; set; }
		//public Nullable<int> OperationsUserId { get; set; }
		//public Nullable<int> PropertyWorkTypeId { get; set; }
		//public int DefaultPropertyFlatRateId { get; set; }
		//public int JobPropertyFlatRateId { get; set; }
		//public bool JobIsTonnage { get; set; }
		//public Nullable<double> JobFixedTonnage { get; set; }
		//public string JobServiceName { get; set; }
		//public bool JobRequiresWeighInOut { get; set; }
		//public Nullable<long> LotSaltTimeId { get; set; }
		//public Nullable<int> ActualDriverVendorWorkerId { get; set; }
		
		//public Nullable<System.DateTime> ActualClockIn { get; set; }
		//public Nullable<System.DateTime> ActualClockOut { get; set; }
		//public Nullable<double> ActualWeightIn { get; set; }
		//public Nullable<double> ActualWeightOut { get; set; }
		//public Nullable<int> ActualPropertyFlatRateId { get; set; }
		//public Nullable<double> ActualTonnage { get; set; }
		//public int TripStatusId { get; set; }
		//public string Comments { get; set; }
		//public string CreatedBy { get; set; }
		//public System.DateTime CreatedOn { get; set; }
		//public string UpdatedBy { get; set; }
		//public Nullable<System.DateTime> UpdatedOn { get; set; }

		//public virtual PropertyFlatRate PropertyFlatRate { get; set; }
		//public virtual PropertyFlatRate PropertyFlatRate1 { get; set; }
		//public virtual PropertyFlatRate PropertyFlatRate2 { get; set; }
		//public virtual LotSaltTime LotSaltTime { get; set; }
		//public virtual User User { get; set; }
		//public virtual PropertyDto Property { get; set; }
		//public virtual PropertyAddress PropertyAddress { get; set; }
		//public virtual PropertyWorkType PropertyWorkType { get; set; }
		//public virtual User User1 { get; set; }
		//public virtual TripRoute TripRoute { get; set; }
		//public virtual TripStatus TripStatus { get; set; }
		//public virtual VendorProperty VendorProperty { get; set; }
		//public virtual VendorRoute VendorRoute { get; set; }
		//public virtual VendorWorker VendorWorker { get; set; }
		//public virtual WorkEvent WorkEvent { get; set; }

		
	}

}