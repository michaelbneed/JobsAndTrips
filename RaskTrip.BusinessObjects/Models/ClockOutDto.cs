using System;
using System.Collections.Generic;
using System.Text;

namespace RaskTrip.BusinessObjects.Models
{
    public class ClockOutDto
    {
        public long JobId { get; set; }

        /// <summary>
        /// The local time recorded by the mobile app that the clock-out action was completed (all prompts answered)
        /// </summary>
        public DateTime ActualClockOut { get; set; }

        /// <summary>
        /// The name/text of the choce made: If the requested service was performed, then use the JobDto.JobServiceName. 
        /// If one of the choices was selected, use the name of the choice: STANDARD/PARTIAL, FULL, SKIPPED - NO ACCESS, or SKIPPED - NOT PLOWED
        /// </summary>
        public string ActualServicePerformed { get; set; }

        /// <summary>
        /// If JobRequiresWeighInOut is true and the job is being completed (i.e. not skipped), the ActualWeightOut should have a value. Otherwise, it should be null.
        /// </summary>
        public double? ActualWeightOut { get; set; }

    }
}
