using System;
using MyShuttle.Client.Core.Infrastructure.Abstractions.Services;
using MyShuttle.Client.Core.Model;
using System.Threading.Tasks;

namespace MyShuttle.Client.Core.Infrastructure
{
    public class LocationServiceSingleton : ILocationServiceSingleton
    {
        private readonly IApplicationSettingServiceSingleton _applicationSettingService;
        
        private Location _lastLocation;

        public LocationServiceSingleton(IApplicationSettingServiceSingleton applicationSettingService)
        {
            if (applicationSettingService == null)
            {
                throw new ArgumentNullException("applicationSettingService");
            }

            _applicationSettingService = applicationSettingService;
        }

        public async Task<Location> CalculatePositionAsync()
        {
            if (_lastLocation != null)
            {
                return _lastLocation;
            }

            if (_applicationSettingService.LocationFixed)
            {
                _lastLocation = new Location 
                { 
                    Latitude = _applicationSettingService.LocationFixedLatitude, 
                    Longitude = _applicationSettingService.LocationFixedLongitude 
                };
            }
            else
            {
                _lastLocation = await CalculatePositionInternalAsync();
            }

            return _lastLocation;
        }

        private Task<Location> CalculatePositionInternalAsync()
        {
            var tcs = new TaskCompletionSource<Location>();

            //_watcher.Start(
            //    new MvxLocationOptions
            //    {
            //        Accuracy = MvxLocationAccuracy.Fine,
            //        MovementThresholdInM = 150,
            //        TimeBetweenUpdates = new TimeSpan(0, 0, 0, 15)
            //    },
            //    location => tcs.TrySetResult(new Location { Latitude = location.Coordinates.Latitude, Longitude = location.Coordinates.Longitude }),
            //    // TODO
            //    error => tcs.TrySetException(new Exception(error.Code.ToString())));
            
            return tcs.Task;
        }
    }
}
