using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Core.Infrastructure.Abstractions.Services;
using MyShuttle.Client.Core.ServiceAgents.Interfaces;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyShuttle.Client.Core.ViewModels
{
    public class VehiclesInMapViewModel : VehiclesViewModelBase, IVehiclesInMapViewModel, INotifyPropertyChanged
    {
        private Vehicle _selectedVehicle;
        private ObservableCollection<Vehicle> _selectedVehicles;

        private readonly ICommand _switchSelectedVehicleCommand = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Vehicle SelectedVehicle
        {
            get { return _selectedVehicle; }
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Vehicle> SelectedVehicles
        {
            get
            {
                return _selectedVehicles;
            }
            set
            {
                _selectedVehicles = value;
                OnPropertyChanged();
            }
        }

        public ICommand SwitchSelectedVehicleCommand
        {
            get { return _switchSelectedVehicleCommand; }
        }

        public VehiclesInMapViewModel(
            IMyShuttleClient myShuttleClient,
            ILocationServiceSingleton locationService,
            IApplicationSettingServiceSingleton applicationSettingService)
            : base(myShuttleClient, locationService, applicationSettingService)
        {

        }

        private void SwitchSelectedVehicle(int vehicleId)
        {
            foreach (var vehicle in this.FilteredVehicles)
            {
                vehicle.IsSelected = false;
            }

            this.SelectedVehicle = this.FilteredVehicles.FirstOrDefault(v => v.VehicleId == vehicleId);

            if (this.SelectedVehicles == null)
            {
                this.SelectedVehicles = new ObservableCollection<Vehicle>();
            }

            this.SelectedVehicles.Clear();
            this.SelectedVehicles.Add(this._selectedVehicle);
            
            if (this.SelectedVehicle != null)
            {
                this.SelectedVehicle.IsSelected = true;

                // This is a "workaround" made in order to raise the vehicles IsSelected property changed to the view.
                var filteredVehiclesAux = this.FilteredVehicles;
                this.FilteredVehicles = null;
                this.FilteredVehicles = filteredVehiclesAux;
            }
        }
    }
}
