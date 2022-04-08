using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevSpector.Desktop.Service;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class FreeIPListViewModel : ListViewModelBase<string>, IFreeIPListViewModel, IInitializableListViewModel<string>
    {
        private readonly IDevicesStorage _storage;

        private readonly IMessagesBroker _messagesBroker;

        private readonly IDevicesListViewModel _devicesListViewModel;

        public FreeIPListViewModel(
            IDevicesStorage storage,
            IMessagesBroker messagesBroker,
            IDevicesListViewModel devicesListViewModel
        )
        {
            _storage = storage;
            _messagesBroker = messagesBroker;
            _devicesListViewModel = devicesListViewModel;
        }

        public override string SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public async void UpdateList()
        {
            try
            {
                List<string> updatedList = await _storage.GetFreeIPAsync();

                await Task.Run(() => {
                    ItemsCache.Clear();
                    foreach (var ip in updatedList)
                        ItemsCache.Add(ip);

                    ItemsToDisplay.Clear();
                    foreach (var ip in ItemsCache)
                        ItemsToDisplay.Add(ip);
                });
            }
            catch (Exception e)
            {
                _messagesBroker.NotifyUser(e.Message);
            }
        }
    }
}
