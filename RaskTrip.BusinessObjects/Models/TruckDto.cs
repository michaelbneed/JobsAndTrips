using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaskTrip.BusinessObjects.Models
{
	/// <summary>
	/// Purpose: Use this during registration of a truck. This DTO verifies that a valid pairing of a TruckNumber with an ApiKey is being made.
	/// The TruckId should be 0 when a request is being made. It will be non-zero when the request is successful.
	/// The Message is returned by the API to indicate, in human readable terms, whether it is successful or not.
	public class TruckDto
	{
		/// <summary>
		/// The unique identifier for a Truck in the R.A.S.K. Business System.  Returned by the API. Used in all requests by the mobile device once successful registration is accomplished.
		/// </summary>
		public int TruckId { get; set; }

		/// <summary>
		/// This is also a unique identification of a vendor's truck.
		/// </summary>
		public string TruckNumber { get; set; }

		/// <summary>
		/// The plain-text key provided by the administrator, to be entered when installing the mobile device in a truck. 
		/// It is used to authenticate the mobile app that is tied to a particular truck.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// A human-readable message: "Successful Registration" or "Not Authorized" 
		/// </summary>
		public string Message { get; set; }
	}
}