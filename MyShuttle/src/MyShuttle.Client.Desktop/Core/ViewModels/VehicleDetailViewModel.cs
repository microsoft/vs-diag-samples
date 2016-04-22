using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Core.Infrastructure.Abstractions.Services;
using MyShuttle.Client.Core.Model;
using MyShuttle.Client.Core.ServiceAgents.Interfaces;
using MyShuttle.Client.Core.ViewModels.Base;
using MyShuttle.Client.Core.ViewModels.Behavoirs;
using MyShuttle.Client.Core.ViewModels.InterfacesForDependencyInjection;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyShuttle.Client.Core.ViewModels
{
    public class VehicleDetailViewModel : NavegableViewModel, INotifyPropertyChanged
    {
        private readonly IMyShuttleClient _myShuttleClient;
        private readonly ILocationServiceSingleton _locationService;
        
        private bool _isLoadingVehicle;
        private Vehicle _currentVehicle;
        private Location _currentLocation;

        private ICommand _callVehicleCommand = null;
        private ICommand _requestVehicleCommand = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public IApplicationSettingServiceSingleton ApplicationSettingService { get; private set; }

        public bool IsLoadingVehicle
        {
            get
            {
                return _isLoadingVehicle;
            }

            set
            {
                _isLoadingVehicle = value;
                OnPropertyChanged();
            }
        }

        public Vehicle CurrentVehicle
        {
            get
            {
                return _currentVehicle;
            }
            set
            {
                _currentVehicle = value;
                OnPropertyChanged();
            }
        }
        
        public Location CurrentLocation
        {
            get
            {
                return _currentLocation;
            }

            set
            {
                _currentLocation = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand CallVehicleCommand
        {
            get { return this._callVehicleCommand; }
        }

        public ICommand RequestVehicleCommand
        {
            get { return this._requestVehicleCommand; }
        }
        
        public VehicleDetailViewModel(
            IMyShuttleClient myShuttleClient,
            //IUserInteraction userInteraction,
            ILocationServiceSingleton locationService,
            IApplicationSettingServiceSingleton applicationSettingService)
        {
            if (myShuttleClient == null)
            {
                throw new ArgumentNullException("myShuttleClient");
            }

            //if (userInteraction == null)
            //{
            //    throw new ArgumentNullException("userInteraction");
            //}

            if (locationService == null)
            {
                throw new ArgumentNullException("locationService");
            }

            if (applicationSettingService == null)
            {
                throw new ArgumentNullException("applicationSettingService");
            }
            
            _myShuttleClient = myShuttleClient;
            //_userInteraction = userInteraction;
            _locationService = locationService;
            ApplicationSettingService = applicationSettingService;

            this.InitializeCommands();
        }

        public async void Init(int currentVehicleId)
        {
            CurrentLocation = await _locationService.CalculatePositionAsync();
            this.LoadVehicleAsync(currentVehicleId);
        }

        private void InitializeCommands()
        {
            //this._callVehicleCommand = new MvxCommand(this.CallVehicle);
            //this._requestVehicleCommand = new MvxCommand(this.RequestVehicle);
        }

        private void CallVehicle()
        {
            var driver = this.CurrentVehicle.Driver;
            if (string.IsNullOrWhiteSpace(driver.Phone))
            {
                return;
            }

            //this._phoneCallTask.MakePhoneCall(driver.Name ?? string.Empty, driver.Phone);
        }

        private void RequestVehicle()
        {
            //await _userInteraction.AlertAsync("Vehicle successfully requested. Thanks!");
        }

        private async void LoadVehicleAsync(int vehicleId)
        {
            this.IsLoadingVehicle = true;
            this.CurrentVehicle = await this._myShuttleClient.VehiclesService.GetAsync(vehicleId);
            this.IsLoadingVehicle = false;
        }
    }
}
