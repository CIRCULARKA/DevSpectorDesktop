using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DevSpector.Desktop.UI.ViewModels;

namespace DevSpector.Desktop.UI.Views
{
    public partial class UsersListView : UserControl
    {
        public UsersListView() { }

        public UsersListView(IUsersListViewModel viewModel)
        {
            DataContext = viewModel;

            AvaloniaXamlLoader.Load(this);
        }
    }
}
