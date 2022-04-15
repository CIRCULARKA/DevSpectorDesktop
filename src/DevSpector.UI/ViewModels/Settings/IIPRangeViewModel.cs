using System.Reactive;
using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IIPRangeViewModel
    {
        int Mask { get; set; }

        string NetworkAddress { get; set; }

        ReactiveCommand<Unit, Unit> GenerateIPRangeCommand { get; }

        void DisplayCurrentNetworkInfo();
    }
}
