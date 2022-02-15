using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ICommonInfoViewModel : IDeviceInfoViewModel
    {
        string InventoryNumber { get; set; }

        string Type { get; set; }
    }
}
