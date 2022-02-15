using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class MainView : Window
    {
        public MainView() { }

        public MainView(IMainViewModel viewModel)
        {
            AvaloniaXamlLoader.Load(this);

            this.AttachDevTools();

            DataContext = viewModel;
        }
    }
}
