using Avalonia.Controls;
using DevSpector.Desktop.UI.Views;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class UsersMainViewModel : ViewModelBase, IUsersMainViewModel
    {
        public UsersMainViewModel(
            UsersListView usersList,
            UserInfoView userInfo,
            SearchView search
        )
        {
            UsersList = usersList;
            UserInfo = userInfo;
            Search = search;
        }

        public UserControl UsersList { get; }

        public UserControl UserInfo { get; }

        public UserControl Search { get; }
    }
}
