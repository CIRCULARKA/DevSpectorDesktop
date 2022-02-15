using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public interface IUserSession
    {
        string Login { get; }

        string AccessToken { get; }

        void StartSession(User user);
    }
}
