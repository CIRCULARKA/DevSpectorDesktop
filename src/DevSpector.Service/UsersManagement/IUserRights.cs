using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public interface IUserRights
    {
        void SetUser(User user);

        User User { get; }

        bool HasAccessToUsers { get; }

        bool CanEditDevices { get; }

        bool CanUpdateIPRange { get; }
    }
}
