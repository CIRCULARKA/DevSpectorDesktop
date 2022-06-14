using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactiveUI;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class DeviceSearchViewModel : ViewModelBase, IDeviceSearchViewModel
    {
        private string _searchQuery;

        private ApplicationEvents _events;

        private IDevicesListViewModel _devicesListViewModel;

        public DeviceSearchViewModel(
            ApplicationEvents events,
            IDevicesListViewModel devicesListViewModel,
            IUserRights userRights
        ) : base(userRights)
        {
            _events = events;
            _devicesListViewModel = devicesListViewModel;

            SearchCommand = ReactiveCommand.CreateFromTask(
                async () => {
                    try
                    {
                        devicesListViewModel.AreItemsLoaded = false;
                        devicesListViewModel.AreThereItems = false;
                        events.RaiseDeviceSearched(
                            await FilterDevicesAsync(devicesListViewModel.ItemsCache)
                        );
                    }
                   finally { devicesListViewModel.AreItemsLoaded = true; }
                }
            );
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
        }

        public ReactiveCommand<Unit, Unit> SearchCommand { get; }

        private Task<IEnumerable<Device>> FilterDevicesAsync(IEnumerable<Device> devices)
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                    return Task.FromResult(new List<Device>(devices).AsEnumerable());

            var result = new List<Device>(devices.Count());

            var filteringTask = Task.Run(
                () => {
                    result.AddRange(devices.Where(d => d.NetworkName.Contains(SearchQuery)));
                    result.AddRange(devices.Where(d => d.InventoryNumber.Contains(SearchQuery)));
                    result.AddRange(devices.Where(d => d.Housing.Contains(SearchQuery)));
                    result.AddRange(devices.Where(d => d.Cabinet.Contains(SearchQuery)));
                    result.AddRange(devices.Where(d => d.Type.Contains(SearchQuery)));

                    return result.AsEnumerable<Device>();
                }
            );

            return filteringTask;
        }
    }
}
