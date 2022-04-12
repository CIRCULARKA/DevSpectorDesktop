using System.Threading.Tasks;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IUsersListViewModel : IListViewModel<User>, IInitializableListViewModel<User>
    {
        Task LoadUserGroupsAsync();
    }
}
