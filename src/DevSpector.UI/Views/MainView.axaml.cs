using Avalonia;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class MainView : RootWindow
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
