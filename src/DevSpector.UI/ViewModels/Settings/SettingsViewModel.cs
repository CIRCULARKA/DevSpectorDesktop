using Avalonia.Controls;
using DevSpector.Desktop.UI.Views;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        public SettingsViewModel(
            AccessKeyView accessKeyView,
            IPRangeView ipRangeView,
            PasswordView passwordView,
            IUserRights userRights
        ) : base(userRights)
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
