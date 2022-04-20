using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.SDK.Providers;
using DevSpector.SDK.Editors;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class LocationInfoViewModel : ViewModelBase, ILocationInfoViewModel
    {
        private List<Housing> _housings;

        private List<Cabinet> _currentCabinets;

        private Dictionary<string, List<Cabinet>> _housingCabinets;

        private Housing _selectedHousing;

        private Cabinet _selectedCabinet;

        private readonly ApplicationEvents _appEvents;

        private readonly IDevicesEditor _devicesEditor;

        private readonly ILocationProvider _locationProvider;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly IMessagesBroker _messagesBroker;

        public LocationInfoViewModel(
<<<<<<< HEAD
            ApplicationEvents appEvents,
            IDevicesStorage storage,
=======
            IApplicationEvents appEvents,
            IDevicesEditor devicesEditor,
            ILocationProvider locationProvider,
>>>>>>> 3ff2313073b30c8e5f328d758efbca0e7f8e700d
            IDevicesListViewModel devicesListViewModel,
            IMessagesBroker messagesBroker,
            IUserRights userRights
        ) : base(userRights)
        {
            _appEvents = appEvents;
            _devicesEditor = devicesEditor;
            _locationProvider = locationProvider;
            _devicesListViewModel = devicesListViewModel;

            _messagesBroker = messagesBroker;

            ApplyChangesCommand = ReactiveCommand.CreateFromTask(
                UpdateDeviceLocationAsync,
                this.WhenAny(
                    (vm) => vm.SelectedHousing,
                    (vm) => vm.SelectedCabinet,
                    (housing, cabinet) => {
                        Device selectedDevice = _devicesListViewModel.SelectedItem;

                        if (selectedDevice == null) return false;
                        if (SelectedHousing == null) return false;

                        if (SelectedCabinet == null) return false;
                        if (SelectedCabinet.CabinetName == selectedDevice.Cabinet) return false;

                        return true;
                    }
                )
            );
        }

        public ReactiveCommand<Unit, Unit> ApplyChangesCommand { get; }

        public List<Housing> Housings
        {
            get => _housings;
            set => this.RaiseAndSetIfChanged(ref _housings, value);
        }

        public Housing SelectedHousing
        {
            get => _selectedHousing;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedHousing, value);
                if (_selectedHousing != null && _housingCabinets != null)
                {
                    Cabinets = _housingCabinets[_selectedHousing.HousingName];
                    SelectedCabinet = Cabinets.FirstOrDefault();
                }
            }
        }

        public List<Cabinet> Cabinets
        {
            get => _currentCabinets;
            set => this.RaiseAndSetIfChanged(ref _currentCabinets, value);
        }

        public Cabinet SelectedCabinet
        {
            get => _selectedCabinet;
            set => this.RaiseAndSetIfChanged(ref _selectedCabinet, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            try
            {
                if (Housings == null) return;

                SelectedHousing = Housings.FirstOrDefault(h => h.HousingName == target.Housing);

                Cabinets = _housingCabinets[SelectedHousing.HousingName];
                SelectedCabinet = Cabinets.FirstOrDefault(c => c.CabinetName == target.Cabinet);
            }
            catch (Exception) { }
        }

        public async Task LoadHousingsAsync()
        {
            try
            {
                Housings = await _locationProvider.GetHousingsAsync();

                SelectedHousing = Housings.FirstOrDefault();

                await LoadCabinetsAsync();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        public async Task UpdateDeviceLocationAsync()
        {
            try
            {
                Device selectedDevice = _devicesListViewModel.SelectedItem;

                await _devicesEditor.MoveAsync(selectedDevice.InventoryNumber, SelectedCabinet.CabinetID);

                _appEvents.RaiseDeviceUpdated();

                _messagesBroker.NotifyUser(
                    $"Устройство перемещено в кабинет \"{SelectedCabinet.CabinetName}\" " +
                    $"корпуса \"{SelectedHousing.HousingName}\""
                );
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        private async Task LoadCabinetsAsync()
        {
            try
            {
                _housingCabinets = new Dictionary<string, List<Cabinet>>();

                foreach (var housing in Housings)
                    _housingCabinets.Add(
                        housing.HousingName,
                        await _locationProvider.GetHousingCabinetsAsync(housing.HousingID)
                    );

                if (SelectedHousing == null) return;

                Cabinets = _housingCabinets[SelectedHousing.HousingName];
                SelectedCabinet = Cabinets.FirstOrDefault();
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
