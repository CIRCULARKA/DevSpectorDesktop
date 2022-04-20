using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class UserSearchView : UserControl
    {
        public UserSearchView() { }

        public UserSearchView(UserSearchViewModel viewModel)
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = viewModel;
        }
    }
}
