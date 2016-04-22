using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Core.ServiceAgents.Interfaces;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace MyShuttle.Client.Desktop.ViewModels
{
    public class DetailsViewModel : ObservableViewModel
    {
        private Vehicle _selectedVehicle;
        private bool _isLoadingDriver;
        private Driver _selectedDriver;
        private DispatcherTimer m_timer;

        protected IMyShuttleClient MyShuttleClient { get; private set; }

        public StatisticsViewModel Statistics { get; set; }
        public ObservableCollection<Vehicle> Vehicles { get; set; }

        public Vehicle SelectedVehicle
        {
            get
            {
                return _selectedVehicle;
            }
            set
            {
                //IEnumerable<int> i = Enumerable.Range(0, 10);
                #region uninteresting
                if (_selectedVehicle != null)
                    _selectedVehicle.Driver = null;
                IsLoadingDriver = true;
                SelectedDriver = null;
                #endregion

                _selectedVehicle = value;
                RaisePropertyChanged();
                UpdateUI(this, new MyEventArgs());
            }
        }

        private void UpdateUI(object sender, System.EventArgs e)
        {
            Cleanup();
            UpdateSelectedDriver(_selectedVehicle);
        }

        #region helpers
        private void Cleanup()
        {
            m_timer.IsEnabled = false;
            m_timer.Tick -= UpdateUI;
            m_timer = null;
        }

        private void UpdateUI(DetailsViewModel o, MyEventArgs a)
        {
            m_timer = new DispatcherTimer();
            m_timer.Interval = new System.TimeSpan(1);
            m_timer.Tick += UpdateUI;
            m_timer.IsEnabled = true;
        }
        #endregion

        public Driver SelectedDriver
        {
            get
            {
                return _selectedDriver;
            }
            set
            {
                _selectedDriver = value;
                RaisePropertyChanged();
            }
        }

        public bool IsLoadingDriver
        {
            get
            {
                return _isLoadingDriver;
            }
            set
            {
                _isLoadingDriver = value;
                RaisePropertyChanged();
            }
        }

        public DetailsViewModel(IMyShuttleClient myShuttleClient, StatisticsViewModel statistics)
        {
            MyShuttleClient = myShuttleClient;
            Statistics = statistics;
        }

        private void UpdateSelectedDriver(Vehicle selectedVehicle)
        {
            SelectedDriver = null;

            if (selectedVehicle == null)
            {
                IsLoadingDriver = false;
                return;
            }

            IsLoadingDriver = true;
            selectedVehicle.Driver =
              MyShuttleClient.DriversService.GetDriver(selectedVehicle.DriverId);

            if (SelectedVehicle == selectedVehicle)
            {
                SelectedDriver = selectedVehicle.Driver;
                EditDriverViewModel.CurrentDriver = SelectedDriver;
                IsLoadingDriver = false;
            }
        }
    }
}
