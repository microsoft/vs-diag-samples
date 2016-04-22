
using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Core.ServiceAgents.Interfaces;
using MyShuttle.Client.Desktop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyShuttle.Client.Desktop.ViewModels
{
    public class CameraViewModel : BaseViewModel
    {
        private bool _isDetecting;
        private Random _random;
        private Task<IEnumerable<Vehicle>> loadVehicleTask;

        protected IMyShuttleClient MyShuttleClient { get; private set; }

        public DetailsViewModel Details { get; set; }

        public Visibility IsEditDriverVisible
        {
            get { return Visibility.Collapsed; }
        }

        public bool IsDetecting
        {
            get
            {
                return _isDetecting;
            }
            set
            {
                _isDetecting = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand DetectingCompletedCommand { get; set; }

        public RelayCommand StartDetectingCommand { get; set; }

        public CameraViewModel(IMyShuttleClient myShuttleClient, DetailsViewModel details)
        {
            _random = new Random((int)DateTime.Now.Ticks);
            MyShuttleClient = myShuttleClient;
            Details = details;
            DetectingCompletedCommand = new RelayCommand(DetectingCompleted);
            StartDetectingCommand = new RelayCommand(StartDetecting);
        }

        private async void StartDetecting()
        {
            IsDetecting = true;
            var vehiclesCount = await MyShuttleClient.VehiclesService.GetCountAsync(string.Empty);
            loadVehicleTask = MyShuttleClient.VehiclesService.GetAsync(string.Empty, 1, _random.Next(0, vehiclesCount));
        }

        private async void DetectingCompleted()
        {
            if (IsDetecting)
            {
                Details.SelectedVehicle = (await loadVehicleTask).FirstOrDefault();
            }
        }

        public override void Load()
        {
            IsDetecting = false;
        }

        public override void Update(string view)
        {
            IsDetecting = false;
            Details.SelectedVehicle = null;
        }
    }
}
