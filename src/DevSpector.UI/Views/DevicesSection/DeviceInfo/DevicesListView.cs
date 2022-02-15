using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class DevicesListView : UserControl
    {
        public DevicesListView() { }

        public DevicesListView(IDevicesListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
