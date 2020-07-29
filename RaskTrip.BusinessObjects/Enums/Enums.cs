using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trip.BusinessObjects.Enums
{
	public class Enums
	{
		public enum TripStatusEnum
		{
			NotStarted = 1,
			Dispatched = 2,
			InProcess = 3,
			SkippedNotPlowed = 4,
			SkippedNoAccess = 5,
			Completed = 6
		}
	}
}
