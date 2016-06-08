using System.Collections.ObjectModel;
using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.UniversalApp.Model;

namespace MyShuttle.Client.Core.ViewModels.InterfacesForDependencyInjection
{
    public interface ICompanyRidesViewModel
    {
        bool IsLoadingLastCompanyRides { get; }

        ObservableCollection<GroupRide> LastCompanyRidesGrouped { get; set; }
    }
}
