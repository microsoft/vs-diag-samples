using System.Windows.Input;
using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Core.Infrastructure.Abstractions.Services;
using MyShuttle.Client.Core.ServiceAgents.Interfaces;
using System;
using System.Collections.ObjectModel;
using MyShuttle.Client.Core.ViewModels.InterfacesForDependencyInjection;
using MyShuttle.Client.Core.ViewModels;
using System.Collections.Generic;
using MyShuttle.Client.UniversalApp.Model;

namespace MyShuttle.Client.Core.ViewModels
{
    //public class CompanyRidesViewModel : MvxViewModel, ICompanyRidesViewModel
    //{
    //    private readonly IMyShuttleClient _myShuttleClient;
    //    private readonly IApplicationSettingServiceSingleton _applicationSettingService;
    //    //private readonly IMvxMessenger _messenger;
    //    //private readonly MvxSubscriptionToken _token;

    //    private bool _isLoadingLastCompanyRides;
    //    private ObservableCollection<GroupRide> _lastCompanyRidesGrouped;

    //    public ICommand _navigateToRideDetailsCommand;
    //    public ICommand _navigateToRideDetailsAlternativeCommand;

    //    public bool IsLoadingLastCompanyRides
    //    {
    //        get
    //        {
    //            return _isLoadingLastCompanyRides;
    //        }
    //        private set
    //        {
    //            _isLoadingLastCompanyRides = value;
    //            RaisePropertyChanged(() => IsLoadingLastCompanyRides);
    //        }
    //    }


    //    public ObservableCollection<GroupRide> LastCompanyRidesGrouped
    //    {
    //        get
    //        {
    //            return _lastCompanyRidesGrouped;
    //        }
    //        set
    //        {
    //            _lastCompanyRidesGrouped = value;
    //            RaisePropertyChanged(() => LastCompanyRidesGrouped);
    //        }
    //    }


    //    public ICommand NavigateToRideDetailsCommand
    //    {
    //        get { return _navigateToRideDetailsCommand; }
    //    }

    //    public ICommand NavigateToRideDetailsAlternativeCommand
    //    {
    //        get { return _navigateToRideDetailsAlternativeCommand; }
    //    }

    //    public CompanyRidesViewModel(IMyShuttleClient myShuttleClient, 
    //        IApplicationSettingServiceSingleton applicationSettingService)
    //    {
    //        if (myShuttleClient == null)
    //        {
    //            throw new ArgumentNullException("myShuttleClient");
    //        }

    //        if (applicationSettingService == null)
    //        {
    //            throw new ArgumentNullException("applicationSettingService");
    //        }

    //        _myShuttleClient = myShuttleClient;
    //        _applicationSettingService = applicationSettingService;
            
    //        InitializeActions();

    //        InitializeCommands();
    //    }

    //    private void InitializeActions()
    //    {
    //        IsLoadingLastCompanyRides = true;
    //        LoadLastCompanyRidesAsync();
    //    }

    //    private void InitializeCommands()
    //    {
    //        _navigateToRideDetailsCommand = new MvxCommand<int>(NavigateToRideDetails);
    //        _navigateToRideDetailsAlternativeCommand = new MvxCommand<Ride>(ride =>
    //            this.NavigateToRideDetails(ride.RideId));
    //    }

    //    private void NavigateToRideDetails(int rideId)
    //    {
    //        ShowViewModel<RideDetailViewModel>(new { currentRideId = rideId });
    //    }

    //    private async void LoadLastCompanyRidesAsync()
    //    {
    //        var rides = new ObservableCollection<Ride>(await _myShuttleClient.RidesService.GetCompanyRidesAsync(_applicationSettingService.TopListItemsCount));
    //        var GroupRides = new GroupRide(rides);
    //        this.LastCompanyRidesGrouped = new ObservableCollection<GroupRide> { GroupRides };     
            
    //        IsLoadingLastCompanyRides = false;
    //    }
    //}
}
