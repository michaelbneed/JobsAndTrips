namespace RaskTrip.BusinessObjects.Models
{
	/// <summary>
	/// The JobDto communicates the next job to perform to the mobile app. It includes all information to be displayed, and to be used for navigation to the site, 
	/// and displaying the map of the site where service is to be performed. The JobId is required for all subsequent API calls concerning this job.
	/// </summary>
	public class JobDto
	{
		/// <summary>
		/// The primary key to identify the job.
		/// </summary>
		public long JobId { get; set; }
		/// <summary>
		/// The short name of the service to be performed. To be displayed to the driver upon arrival at the site.
		/// </summary>
		public string JobServiceName { get; set; }
		/// <summary>
		/// Special Instructions for servicing the site, if any (empty string if not). To be displayed to the driver upon arrival at the site.
		/// </summary>
		public string SpecialInstructions { get; set; }

		/// <summary>
		/// The Name of the Property -- could be a business name, building name, or a major facility such as a shopping mall or sports venue
		/// </summary>
		public string PropertyName { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }

		/// <summary>
		/// The sales rep responsible for the property
		/// </summary>
		public string SalesRepContactName { get; set; }
		public string SalesRepPhone { get; set; }

		/// <summary>
		/// The operations manager/supervisor/foreman who assigned this Job.
		/// </summary>
		public string OperationsContactName { get; set; }
		public string OperationsContactPhone { get; set; }

		/// <summary>
		/// The GPS coordinates are required. They are used to determine when the truck (mobile app) is within an acceptable radius of a particular location on the property
		/// where the service is to be performed. It may not be the main entry to the property/building. It will be the specific entrance to the parking lot to be serviced.
		/// </summary>
		public double GpsLatitude { get; set; }
		public double GpsLongitude { get; set; }
		public double GpsRadius { get; set; }

		/// <summary>
		/// If the property requires that the truck weigh-in at check-in time and weigh-out when the work is complete, this will be true. Otherwise, it will be false.
		/// </summary>
		public bool JobRequiresWeighInOut { get; set; }

		/// <summary>
		/// The "Site Map" of the property, detailing where service(s) are to be performed.
		/// </summary>
		public string SiteFotosUrl { get; set; }
	}

}