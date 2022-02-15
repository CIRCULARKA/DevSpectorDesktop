using ReactiveUI;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SoftwareInfoViewModel : ViewModelBase, ISoftwareInfoViewModel
    {
        private string _software;

        public SoftwareInfoViewModel() { }
        public string Software
        {
            get => _software;
            set => this.RaiseAndSetIfChanged(ref _software, value);
        }

        public void UpdateDeviceInfo(Appliance target)
        {
            // Temp
            Software = target?.Software?.Count.ToString();
        }
    }
}
