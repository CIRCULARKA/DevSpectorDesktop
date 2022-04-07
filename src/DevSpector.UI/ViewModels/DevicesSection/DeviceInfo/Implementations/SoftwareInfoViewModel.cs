using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactiveUI;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SoftwareInfoViewModel : ListViewModelBase<Software>, ISoftwareInfoViewModel
    {
        private bool _canInputSoftwareInfo;

        private string _softwareName;

        private string _softwareVersion;

        private List<Software> _software;

        private readonly IDevicesStorage _storage;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly IMessagesBroker _messagesBroker;

        public SoftwareInfoViewModel(
            IDevicesStorage storage,
            IDevicesListViewModel devicesListViewModel,
            IMessagesBroker messagesBroker
        )
        {
            _storage = storage;
            _devicesListViewModel = devicesListViewModel;

            _messagesBroker = messagesBroker;

            AddSoftwareCommand = ReactiveCommand.CreateFromTask(
                AddSoftwareAsync,
                this.WhenAny(
                    (vm) => vm.SoftwareName,
                    (vm) => {
                        if (string.IsNullOrWhiteSpace(SoftwareName)) return false;
                        if (SoftwareName.Length < 3) return false;
                        return true;
                    }
                )
            );

            RemoveSoftwareCommand = ReactiveCommand.CreateFromTask(
                RemoveSoftwareAsync
            );

            SwitchInputFieldsCommand = ReactiveCommand.Create(
                () => { CanInputSoftwareInfo = !CanInputSoftwareInfo; }
            );
        }

        public ReactiveCommand<Unit, Unit> AddSoftwareCommand { get; }

        public ReactiveCommand<Unit, Unit> RemoveSoftwareCommand { get; }

        public ReactiveCommand<Unit, Unit> SwitchInputFieldsCommand { get; }

        public bool CanInputSoftwareInfo
        {
            get => _canInputSoftwareInfo;
            set => this.RaiseAndSetIfChanged(ref _canInputSoftwareInfo, value);
        }

        public string SoftwareName
        {
            get => _softwareName;
            set => this.RaiseAndSetIfChanged(ref _softwareName, value);
        }

        public string SoftwareVersion
        {
            get => _softwareVersion;
            set => this.RaiseAndSetIfChanged(ref _softwareVersion, value);
        }

        public List<Software> Software
        {
            get => _software;
            set => this.RaiseAndSetIfChanged(ref _software, value);
        }

        public override Software SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            if (target == null)
                Software = null;
            else
                Software = target.Software;
        }

        public async Task AddSoftwareAsync()
        {
            try
            {
                Device selectedDevice = _devicesListViewModel.SelectedItem;

                var newSoft = new Software {
                    SoftwareName = SoftwareName,
                    SoftwareVersion = SoftwareVersion
                };

                await _storage.AddSoftwareAsync(
                    selectedDevice.InventoryNumber,
                    newSoft
                );

                AddToList(newSoft);

                if (SoftwareVersion != null)
                    _messagesBroker.NotifyUser($"ПО \"{SoftwareName}\" с версией \"{SoftwareVersion}\" добавлено");
                else
                    _messagesBroker.NotifyUser($"ПО \"{SoftwareName}\" добавлено");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }

        public async Task RemoveSoftwareAsync()
        {
            try
            {
                Device selectedDevice = _devicesListViewModel.SelectedItem;

                await _storage.RemoveSoftwareAsync(selectedDevice.InventoryNumber, _selectedItem);

                Software removedSoftware = SelectedItem;

                RemoveFromList(SelectedItem);

                _messagesBroker.NotifyUser($"ПО \"{removedSoftware.SoftwareName}\" удалено");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
