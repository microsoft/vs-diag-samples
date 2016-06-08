using MyShuttle.Client.Services.Interfaces;

namespace MyShuttle.Client.Core.ServiceAgents.Interfaces
{
    public interface IMyShuttleClient
    {
        IAnalyticsService AnalyticsService { get; }

        ICustomersService CustomersService { get; }

        IEmployeesService EmployeesService { get; }

        ICarriersService CarriersService { get; }

        DriversService DriversService { get; }

        IVehiclesService VehiclesService { get; }

        IRidesService RidesService { get; }


        void Refresh();
    }
}
