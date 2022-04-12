using System.Threading.Tasks;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface IUserInfoViewModel
    {
        string AccessToken { get; set; }

        string Login { get; set; }

        string Group { get; set; }

        void UpdateUserInfo(User target);

        Task LoadUserGroupsAsync();
    }
}
