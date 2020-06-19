using RaskTrip.BusinessObjects.Models;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Collections;
using Xamarin.Forms;

namespace RaskTrip
{
    public class LocationArrivalOptions
    {
        public double ShortDistanceThreshold { get; set; }
        public int ShortDistanceCheckInterval { get; set; }
        public int LongDistanceCheckInterval { get; set; }
    }

    public class LocationArrivalWatcher
    {
        const double FEET_PER_MILE = 5280.0;
        const double DEFAULT_ARRIVAL_RADIUS = 50.0 / FEET_PER_MILE; // 50 feet
        const double DEFAULT_GEOCODED_ARRIVAL_RADIUS = 200.0 / FEET_PER_MILE; // 200 feet

        static LocationArrivalOptions defaultOptions = new LocationArrivalOptions() { 
            ShortDistanceThreshold = 1.0, 
            ShortDistanceCheckInterval = 5, 
            LongDistanceCheckInterval = 30 };

        private JobDto currentJob = null;
        private LocationArrivalOptions options = null;
        private Location targetLocation;
        private double arrivalRadius;
        private Geocoder geocoder = new Geocoder();
        public LocationArrivalWatcher(JobDto job)
        {
            currentJob = job;
            options = defaultOptions;
        }

        public LocationArrivalWatcher(JobDto job, LocationArrivalOptions options)
        {
            currentJob = job;
            this.options = options;
        }

        public async Task<bool> Run()
        {
            var target = await GetTargetLocation().ConfigureAwait(false);
            var current = await GetLocationAsync().ConfigureAwait(false);
            var distance = Distance(current, target);
            while (targetLocation.Latitude != 0 && targetLocation.Longitude != 0 && !HaveArrived(distance, arrivalRadius))
            {
                int delaySeconds = (distance <= this.options.ShortDistanceThreshold) ? options.ShortDistanceCheckInterval : options.LongDistanceCheckInterval;
                await Task.Delay(delaySeconds * 1000);

                current = await GetLocationAsync().ConfigureAwait(false);
                distance = Distance(current, target);
            }
            return (targetLocation.Latitude != 0 && targetLocation.Longitude != 0 && HaveArrived(distance, arrivalRadius));
        }

        public static async Task<Location> GetLocationAsync()
        {
            try
            {
                //var request = new GeolocationRequest(GeolocationAccuracy.High, new TimeSpan(0, 0, 2));
                return await Geolocation.GetLastKnownLocationAsync().ConfigureAwait(false);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine($"GeoLocation GetLocation feature is not supported: {fnsEx.Message}");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Console.WriteLine($"Geolocation GetLocation feature is not enabled: {fneEx.Message}");
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine($"Geolocation GetLocation feature is not permitted: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get location: {ex.Message}");
            }
            return new Location();
        }   

        public static Location GetLocation()
        {
            try
            {
                return Geolocation.GetLastKnownLocationAsync().Result;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine($"GeoLocation GetLocation feature is not supported: {fnsEx.Message}");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Console.WriteLine($"Geolocation GetLocation feature is not enabled: {fneEx.Message}");
            }
            catch (PermissionException pEx)
            {
                Console.WriteLine($"Geolocation GetLocation feature is not permitted: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get location: {ex.Message}");
            }
            return new Location();
        }
        public static bool HaveArrived(double distance, double radius)
        {
            return (Math.Abs(distance) <= Math.Abs(radius));
        }

        public static double Distance(Location p1, Location p2)
        {
            double result = 0.0;
            result = Math.Abs(p1.CalculateDistance(p2, DistanceUnits.Miles));
            return result;
        }

        private async Task<Location> GetTargetLocation()
        {
            if (currentJob == null)
            {
                arrivalRadius = DEFAULT_ARRIVAL_RADIUS;
                targetLocation = new Location();
            }
            else if (currentJob.GpsLatitude != 0.0 && currentJob.GpsLongitude != 0.0)
            {
                arrivalRadius = currentJob.GpsRadius == 0.0 ? DEFAULT_ARRIVAL_RADIUS : currentJob.GpsRadius/FEET_PER_MILE;
                targetLocation = new Location(currentJob.GpsLatitude, currentJob.GpsLongitude);
            }
            else
            {
                string targetAddress = (currentJob.Street1 ?? "") + " " + (currentJob.Street2 ?? "") + " " + (currentJob.City ?? "") + " " + (currentJob.State ?? "") + " " + (currentJob.ZipCode ?? "");
                var targetPositions = await geocoder.GetPositionsForAddressAsync(targetAddress).ConfigureAwait(false);
                if (targetPositions != null && targetPositions.Count() >= 1)
                {
                    arrivalRadius = currentJob.GpsRadius == 0.0 ? DEFAULT_GEOCODED_ARRIVAL_RADIUS : currentJob.GpsRadius/FEET_PER_MILE;

                    var pos = targetPositions.First();
                    targetLocation = new Location(pos.Latitude, pos.Longitude);
                }
            }
            return targetLocation;
        }
    }
}
