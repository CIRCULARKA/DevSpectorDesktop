using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class DevicesMainView : UserControl
    {
        public DevicesMainView() { }

        public DevicesMainView(IDevicesMainViewModel viewModel)
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = viewModel;
        }
    }
}
