using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public class UserSession : IUserSession
    {
        private readonly ApplicationEvents _events;

        private User _loggedUser;

        public UserSession(ApplicationEvents events)
        {
            _events = events;
        }

        public string Login => _loggedUser?.Login;

        public string Group => _loggedUser?.Group;

        public string AccessToken => _loggedUser?.AccessToken;

        public void StartSession(User user)
        {
            _loggedUser = user;

            _events.RaiseUserAuthorized(user);
        }

        public void EndSession()
        {
            _events.RaiseLogout();
        }
    }
}
