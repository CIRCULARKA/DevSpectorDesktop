using System.Threading.Tasks;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ICommonInfoViewModel : IDeviceInfoViewModel
    {
        string InventoryNumber { get; set; }

        DeviceType SelectedDeviceType { get; set; }

        Task LoadDeviceTypesAsync();
    }
}
