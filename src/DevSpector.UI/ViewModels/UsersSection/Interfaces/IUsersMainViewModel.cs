using Avalonia.Controls;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IUsersMainViewModel
    {
        UserControl UsersList { get; }

        UserControl Search { get; }
    }
}
