using ReactiveUI;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class NetworkInfoViewModel : ListViewModelBase<string>, INetworkInfoViewModel
    {
        public NetworkInfoViewModel() { }

        public override string SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            if (target == null) return;

            Items.Clear();
            foreach (var ip in target.IPAddresses)
                Items.Add(ip);
        }
    }
}
