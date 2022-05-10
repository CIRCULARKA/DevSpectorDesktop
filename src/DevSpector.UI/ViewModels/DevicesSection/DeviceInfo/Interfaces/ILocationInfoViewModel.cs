using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ILocationInfoViewModel : IDeviceInfoViewModel
    {
        void UpdateInputsAccessibility();

        List<Housing> Housings { get; set; }

        List<Cabinet> Cabinets { get; set; }

        Task LoadHousingsAsync();
    }
}
