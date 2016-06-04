using System.Collections.ObjectModel;
using Cirrious.MvvmCross.ViewModels;
using MyShuttle.Client.Core.DocumentResponse;

namespace MyShuttle.Client.Core.ViewModels.InterfacesForDependencyInjection
{
    public interface IRideDetailViewModel : IMvxViewModel
    {
        bool IsLoadingRide { get; }

        Ride Ride { get; set; }
    }
}
