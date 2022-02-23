using ReactiveUI;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class UserInfoViewModel : ViewModelBase, IUserInfoViewModel
    {
        private string _accessToken;

        private string _login;

        private string _group;

        public UserInfoViewModel() { }

        public string AccessToken
        {
            get { return _accessToken == null ? "" : _accessToken; }
            set => this.RaiseAndSetIfChanged(ref _accessToken, value);
        }

        public string Group
        {
            get { return _group == null ? "N/A" : _group; }
            set => this.RaiseAndSetIfChanged(ref _group, value);
        }

        public string Login
        {
            get { return _login == null ? "N/A" : _login; }
            set => this.RaiseAndSetIfChanged(ref _login, value);
        }

        public void UpdateUserInfo(User target)
        {
            AccessToken = target?.AccessToken;
            Group = target?.Group;
            Login = target?.Login;
        }
    }
}
