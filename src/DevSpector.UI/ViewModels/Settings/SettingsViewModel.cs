using Avalonia.Controls;
using DevSpector.Desktop.UI.Views;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public SettingsViewModel(
            AccessKeyView accessKeyView,
            IPRangeView ipRangeView,
            PasswordView passwordView
        )
        {
            AccessKeyView = accessKeyView;
            IPRangeView = ipRangeView;
            PasswordView = passwordView;
        }

        public UserControl AccessKeyView { get; }

        public UserControl IPRangeView { get; }

        public UserControl PasswordView { get; }
    }
}
