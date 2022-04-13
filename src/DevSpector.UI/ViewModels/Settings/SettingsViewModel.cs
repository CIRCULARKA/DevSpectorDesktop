using Avalonia.Controls;
using DevSpector.Desktop.UI.Views;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public SettingsViewModel(
            AccessKeyView accessKeyView,
            IPRangeView ipRangeView
        )
        {
            AccessKeyView = accessKeyView;
            IPRangeView = ipRangeView;
        }

        public UserControl AccessKeyView { get; set; }

        public UserControl IPRangeView { get; set; }
    }
}
