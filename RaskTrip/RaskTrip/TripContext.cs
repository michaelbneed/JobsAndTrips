
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RaskTrip.BusinessObjects.Models;
using RaskTrip.ApiClient;

namespace RaskTrip
{
    public static class TripContext
    {
        public static TruckDto Credentials = null;
        public static JobDto CurrentJob = null;
        public static string CurrentPage = String.Empty;
        public static string ClockInOutState = String.Empty;
    
        public static async Task<bool> GetNextJob()
        {
            bool jobChanged = false;
            if (Credentials == null || Credentials.TruckId == 0)
                throw new Exception("GetNextJob requires API Credentials to be established. Restart the App and ensure your truck is registered.");

            var previousJobId = CurrentJob?.JobId;
            CurrentJob = null;
            CurrentJob = await Task.Run<JobDto>(async () => {
                var api = new ApiClient.ApiClient(TripContext.Credentials.TruckNumber, TripContext.Credentials.ApiKey);
                return await api.GetNextJobAsync(TripContext.Credentials.TruckId);
            });
            jobChanged = (previousJobId != CurrentJob?.JobId);
            return jobChanged;
        }
        public static async Task<bool> EstablishVerifiedCredentials()
        {
            bool result = false;
            Credentials = null;
            var truckCredentials = CredentialsManager.GetLoginCredentials(); // try running the damn thing synchronously
            if (truckCredentials.TruckId > 0)
            {
                // run api tasks on thread pool
                var validCredentials = await Task.Run<TruckDto>(async () =>
                {
                    return await CredentialsManager.VerifyCredentials(truckCredentials);
                });
                if (validCredentials.TruckId > 0)
                {
                    Credentials = validCredentials;
                    result = true;
                }
            }
            return result;
        }

        public static async Task<TruckDto> VerifyCredentials(TruckDto newTruck)
        {
            var result = await Task.Run<TruckDto>(async () =>
            {
                return await CredentialsManager.VerifyCredentials(newTruck);
            });
            return result;
        }

        public static async Task<bool> ClockIn(ClockInDto clockInDto)
        {
            bool clockedIn = await Task.Run<bool>(async () => {
                var api = new ApiClient.ApiClient(Credentials.TruckNumber, Credentials.ApiKey);
                return await api.ClockInAsync(clockInDto);
            });
            if (clockedIn)
                ClockInOutState = "ClockOut";
            return clockedIn;
        }

        public static async Task<bool> ClockOut(ClockOutDto clockOutDto)
        {
            bool success = await Task.Run<bool>(async () => {
                var api = new ApiClient.ApiClient(Credentials.TruckNumber, Credentials.ApiKey);
                return await api.ClockOutAsync(clockOutDto);
            });
            if (success)
                ClockInOutState = String.Empty;
            return success;
        }
    }
}
