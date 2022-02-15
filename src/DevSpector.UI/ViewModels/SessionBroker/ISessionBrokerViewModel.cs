using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public interface ISessionBrokerViewModel
    {
        string LoggedUserLogin { get; set; }

        void UpdateLoggedUserInfo(User user);
    }
}
