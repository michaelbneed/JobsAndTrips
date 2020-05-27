using System;

namespace RaskTrip.BusinessObjects.Models
{
    public class ClockInDto
    {
        public long JobId { get; set; }
        /// <summary>
        /// The local date and time recorded by the mobile device when the driver clocked-in.
        /// </summary>
        public DateTime ActualClockIn { get; set; }

        /// <summary>
        /// The ActualWeightIn should be null if JobRequiresWeighInOut is false. It may also be null if the driver clocks in but decides to skip servicing the property.
        /// The ActualWeightIn should have a value if JobRequiresWeighInOut is true and the driver intends to service the property. Although, the driver may also decide
        /// at clock-out time that the property was skipped for some reason.  However, to complete a job which requires weigh in/out, both weights must be provided.
        /// </summary>
        public double? ActualWeightIn { get; set; }
    }
}
