using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using Avalonia.Data;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class IPRangeViewModel : ViewModelBase, IIPRangeViewModel
    {
        private int _mask;

        private string _networkAddress;

        private readonly IDevicesStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        public IPRangeViewModel(
            IDevicesStorage devicesStorage,
            IMessagesBroker messagesBroker
        )
        {
            _storage = devicesStorage;

            _messagesBroker = messagesBroker;

            GenerateIPRangeCommand = ReactiveCommand.CreateFromTask(
                UpdateIPRangeAsync,
                this.WhenAny(
                    (vm) => vm.Mask,
                    (vm) => vm.NetworkAddress,
                    (mask, address) => {
                        if (Mask < 20 || Mask > 31) return false;
                        if (string.IsNullOrWhiteSpace(NetworkAddress)) return false;
                        return true;
                    }
                )
            );
        }

        public int Mask
        {
            get => _mask;
            set
            {
                if (value < 20 || value > 31)
                    throw new DataValidationException("Значение должно быть не менее 20 и не более 31");

                this.RaiseAndSetIfChanged(ref _mask, value);
            }
        }

        public string NetworkAddress
        {
            get => _networkAddress;
            set => this.RaiseAndSetIfChanged(ref _networkAddress, value);
        }

        public ReactiveCommand<Unit, Unit> GenerateIPRangeCommand { get; }

        public void DisplayCurrentNetworkInfo()
        {
            // There is no way to examine what currently network address and mask is
            // so it must be implemented on server side first
            Mask = 24;
            NetworkAddress = "";
        }

        private async Task UpdateIPRangeAsync()
        {
            try
            {
                await _storage.UpdateIPRangeAsync(Mask, NetworkAddress);

                _messagesBroker.NotifyUser("Диапазон IP-адресов успешно обновлён");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
