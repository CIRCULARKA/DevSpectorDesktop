using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class FreeIPListView : UserControl
    {
        public FreeIPListView() { }

        public FreeIPListView(IFreeIPListViewModel viewModel)
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = viewModel;
        }
    }
}
