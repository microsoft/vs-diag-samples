namespace MyShuttle.Client.Core.Infrastructure.Abstractions.Services
{
    public interface IApplicationSettingServiceSingleton
    {
        string UrlPrefix { get; }
        string VehicleServiceUrlPrefix { get; }

        bool? AuthenticationEnabled { get; set; }

        string BingMapsToken { get; set; }

        int TopListItemsCount { get; set; }

        bool LocationFixed { get; }

        double LocationFixedLatitude { get; }

        double LocationFixedLongitude { get; }

        void Refresh();
    }
}
