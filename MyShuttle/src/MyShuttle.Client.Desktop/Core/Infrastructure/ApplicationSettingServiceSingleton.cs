//using Cirrious.MvvmCross.Plugins.Messenger;
using MyShuttle.Client.Core.Infrastructure.Abstractions.Repositories;
using MyShuttle.Client.Core.Infrastructure.Abstractions.Services;
using System;

namespace MyShuttle.Client.Core.Infrastructure
{
    public class ApplicationSettingServiceSingleton : IApplicationSettingServiceSingleton
    {
        // Services
        private readonly IApplicationDataRepository _applicationDataRepository;
        //private readonly IMvxMessenger _messenger;

        //private readonly MvxSubscriptionToken _token;

        // Fields
        private const string UrlPrefixKey = "URLPREFIXKEY";
        private const string DefaultUrlPrefixValue = "http://myshuttledev.azurewebsites.net/web/";
        //private const string DefaultUrlPrefixValue = "http://localhost:5000";
        private string _urlPrefix;

        private const string VehicleServiceUrlKey = "VEHICLEURLKEY";
        private string _vehicleServiceUrlPrefix;

        private const string AuthenticationEnabledKey = "AUTHENTICATIONENABLEDKEY";
        private readonly bool? _defaultAuthenticationEnabledValue = null;
        private bool? _authenticationEnabled;

        private const string BingMapsTokenKey = "BINGMAPSTOKENKEY";
        private const string DefaultBingMapsTokenValue = "AiOp5RxYtMGivuaQLXhKiGK4m2xBNk2WRXHWHPbXCVrCHvX2_ozTtMZPxtgO4QRO";
        private string _bingMapsToken;

        private const string TopListItemsCountKey = "TOPLISTITEMSCOUNTKEY";
        private readonly int? _defaultTopListItemsCountValue = 10;
        private int? _topListItemsCount;

        private const bool LocationFixedValue = true;
        private const double LocationFixedLatitudeValue = 40.72109533886229;
        private const double LocationFixedLongitudeValue = -74.006596745813; 

        public string UrlPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_urlPrefix))
                {
                    _urlPrefix = _applicationDataRepository.GetStringFromApplicationData(UrlPrefixKey, DefaultUrlPrefixValue);
                }
                
                return _urlPrefix;
            }
            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value) || value == _urlPrefix)
            //    {
            //        return;
            //    }

            //    _applicationDataRepository.SetStringToApplicationData(UrlPrefixKey, value);
            //    _urlPrefix = value;
            //}
        }

        public string VehicleServiceUrlPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_vehicleServiceUrlPrefix))
                {
                    _vehicleServiceUrlPrefix = _applicationDataRepository.GetStringFromApplicationData(VehicleServiceUrlKey, DefaultUrlPrefixValue);
                }

                return _vehicleServiceUrlPrefix;
            }

            //set
            //{
            //    if (string.IsNullOrWhiteSpace(value) || value == _vehicleServiceUrlPrefix)
            //    {
            //        return;
            //    }

            //    _applicationDataRepository.SetStringToApplicationData(VehicleServiceUrlKey, value);
            //    _vehicleServiceUrlPrefix = value;
            //}
        }

        public bool? AuthenticationEnabled
        {
            get
            {
                if (!_authenticationEnabled.HasValue)
                {
                    _authenticationEnabled = _applicationDataRepository.GetOptionalBooleanFromApplicationData(AuthenticationEnabledKey, _defaultAuthenticationEnabledValue);
                }
                return _authenticationEnabled.Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string BingMapsToken
        {
            get
            {
                if (string.IsNullOrEmpty(_bingMapsToken))
                {
                    _bingMapsToken = _applicationDataRepository.GetStringFromApplicationData(BingMapsTokenKey, DefaultBingMapsTokenValue);
                }
                return _bingMapsToken;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int TopListItemsCount
        {
            get
            {
                if (!_topListItemsCount.HasValue)
                {
                    _topListItemsCount = _applicationDataRepository.GetOptionalIntegerFromApplicationData(TopListItemsCountKey, _defaultTopListItemsCountValue);
                }
                return _topListItemsCount.Value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool LocationFixed
        {
            get { return LocationFixedValue; }
        }

        public double LocationFixedLatitude
        {
            get { return LocationFixedLatitudeValue; }
        }

        public double LocationFixedLongitude
        {
            get { return LocationFixedLongitudeValue; }
        }

        public ApplicationSettingServiceSingleton(
            IApplicationDataRepository applicationDataRepository)
        {
            if (applicationDataRepository == null)
            {
                throw new ArgumentNullException("applicationDataRepository");
            }

            //if (messenger == null)
            //{
            //    throw new ArgumentNullException("messenger");
            //}

            _applicationDataRepository = applicationDataRepository;
            //_messenger = messenger;

            //_token = _messenger.Subscribe<ReloadDataMessage>(_ => MarkUrlPrefixAsDirty());
        }

        public void Refresh()
        {
            _urlPrefix = null;
            _authenticationEnabled = null;
            _topListItemsCount = null;
        }

        private void MarkUrlPrefixAsDirty()
        {
            this._urlPrefix = null;
        }
    }
}
