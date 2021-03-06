using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.SDK.Editors;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SoftwareInfoViewModel : ListViewModelBase<Software>, ISoftwareInfoViewModel
    {
        private bool _canInputSoftwareInfo;

        private string _softwareName;

        private string _softwareVersion;

        private readonly IDevicesListViewModel _devicesListViewModel;

        private readonly IDevicesEditor _devicesEditor;

        private readonly IMessagesBroker _messagesBroker;

        public SoftwareInfoViewModel(
            IDevicesListViewModel devicesListViewModel,
            IDevicesEditor devicesEditor,
            IMessagesBroker messagesBroker,
            IUserRights userRights
        ) : base(userRights)
        {
            _devicesListViewModel = devicesListViewModel;

            _messagesBroker = messagesBroker;

            _devicesEditor = devicesEditor;

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
                RemoveSoftwareAsync,
                this.WhenAny(
                    (vm) => vm.SelectedItem,
                    (selectedSoft) => SelectedItem != null
                )
            );

            SwitchInputFieldsCommand = ReactiveCommand.Create(
                () => { CanInputSoftwareInfo = !CanInputSoftwareInfo; },
                this.WhenAny(
                    (vm) => vm._devicesListViewModel.SelectedItem,
                    (device) => _devicesListViewModel.SelectedItem != null
                )
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

        public override Software SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            if (target == null)
            {
                ItemsCache.Clear();
                ItemsToDisplay.Clear();
                return;
            }

            Task.Run(() => {
                ItemsCache.Clear();
                foreach (var soft in target.Software)
                    ItemsCache.Add(soft);

                ItemsToDisplay.Clear();
                foreach (var soft in target.Software)
                    ItemsToDisplay.Add(soft);
            });
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

                await _devicesEditor.AddSoftwareAsync(
                    selectedDevice.InventoryNumber,
                    newSoft
                );

                AddToList(newSoft);

                if (SoftwareVersion != null)
                    _messagesBroker.NotifyUser($"???? \"{SoftwareName}\" ?? ?????????????? \"{SoftwareVersion}\" ??????????????????");
                else
                    _messagesBroker.NotifyUser($"???? \"{SoftwareName}\" ??????????????????");
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

                await _devicesEditor.RemoveSoftwareAsync(selectedDevice.InventoryNumber, SelectedItem);

                Software removedSoftware = SelectedItem;

                RemoveFromList(SelectedItem);

                _messagesBroker.NotifyUser($"???? \"{removedSoftware.SoftwareName}\" ??????????????");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
