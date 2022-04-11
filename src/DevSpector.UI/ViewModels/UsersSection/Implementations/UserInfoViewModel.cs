using ReactiveUI;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class UserInfoViewModel : ViewModelBase, IUserInfoViewModel
    {
        private string _accessToken;

        private string _login;

        private string _group;

        private string _firstName;

        private string _surname;

        private string _patronymic;


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

        public string FirstName
        {
            get => _firstName;
            set => this.RaiseAndSetIfChanged(ref _firstName, value);
        }

        public string Surname
        {
            get => _surname;
            set => this.RaiseAndSetIfChanged(ref _surname, value);
        }

        public string Patronymic
        {
            get => _patronymic;
            set => this.RaiseAndSetIfChanged(ref _patronymic, value);
        }

        public void UpdateUserInfo(User target)
        {
            AccessToken = target?.AccessToken;
            Group = target?.Group;
            Login = target?.Login;
            FirstName = target?.FirstName;
            Surname = target?.Surname;
            Patronymic = target?.Patronymic;
        }
    }
}
