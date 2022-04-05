using System.Reactive;
using ReactiveUI;
using DevSpector.SDK.Models;
using DevSpector.SDK.Editors;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class CommonInfoViewModel : ViewModelBase, ICommonInfoViewModel
    {
        private string _inventoryNumber;

        private string _type;

        private readonly IDevicesEditor _editor;

        public CommonInfoViewModel(
            IDevicesEditor editor,
            IMessagesBroker messagesBroker
        )
        {
            _editor = editor;
        }

        public ReactiveCommand<Unit, Unit> ApplyChangesCommand { get; }

        public string InventoryNumber
        {
            get { return _inventoryNumber == null ? "N/A" : _inventoryNumber; }
            set => this.RaiseAndSetIfChanged(ref _inventoryNumber, value);
        }

        public string Type
        {
            get { return _type == null ? "N/A" : _type; }
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        public void UpdateDeviceInfo(Device target)
        {
            InventoryNumber = target?.InventoryNumber;
            Type = target?.Type;
        }
    }
}
