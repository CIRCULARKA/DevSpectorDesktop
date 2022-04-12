using Avalonia.Controls;
using DevSpector.Desktop.UI.Views;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public SettingsViewModel(AccessKeyView accessKeyView)
        {
            AccessKeyView = accessKeyView;
        }

        public UserControl AccessKeyView { get; set; }
    }
}
