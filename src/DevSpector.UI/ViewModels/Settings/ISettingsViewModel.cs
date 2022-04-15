using Avalonia.Controls;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ISettingsViewModel
    {
        UserControl AccessKeyView { get; }

        UserControl IPRangeView { get; }

        UserControl PasswordView { get; }
    }
}
