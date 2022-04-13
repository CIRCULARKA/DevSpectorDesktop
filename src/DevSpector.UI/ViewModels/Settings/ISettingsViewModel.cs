using Avalonia.Controls;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ISettingsViewModel
    {
        UserControl AccessKeyView { get; set; }

        UserControl IPRangeView { get; set; }
    }
}
