using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Data;
using ReactiveUI;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Networking;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class IPRangeViewModel : ViewModelBase, IIPRangeViewModel
    {
        private int _mask;

        private string _networkAddress;

        private ApplicationEvents _appEvents;

        private readonly IMessagesBroker _messagesBroker;

        private readonly INetworkManager _networkManager;

        public IPRangeViewModel(
            IMessagesBroker messagesBroker,
            INetworkManager networkManager,
            IUserRights userRights,
            ApplicationEvents appEvents

        ) : base(userRights)
        {
            _messagesBroker = messagesBroker;
            _networkManager = networkManager;
            _appEvents = appEvents;

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
                await _networkManager.GenerateIPRangeAsync(NetworkAddress, Mask);

                _appEvents.RaiseIPRangeUpdated();

                _messagesBroker.NotifyUser("Диапазон IP-адресов успешно обновлён");
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
