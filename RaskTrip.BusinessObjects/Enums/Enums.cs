using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaskTrip.Utility.Helpers
{
	public class Enums
	{
		public enum TripStatusEnum
		{
			NotStarted,
			Dispatched,
			InProcess,
			SkippedNotPlowed,
			SkippedNoAccess,
			Completed
		}
	}
}
