using System;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public class UserRights : IUserRights
    {
        public void SetUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User must be provided in order to define his rights");
            User = user;

            DefineRights();
        }

        public User User { get; private set; }

        public bool HasAccessToUsers { get; private set; }

        public bool CanEditDevices { get; private set; }

        public bool CanUpdateIPRange { get; private set; }

        private void DefineRights()
        {
            switch (User.Group)
            {
                case "Техник":
                    HasAccessToUsers = false;
                    CanEditDevices = false;
                    CanUpdateIPRange = false;
                    break;
                case "Администратор":
                    HasAccessToUsers = false;
                    CanEditDevices = true;
                    CanUpdateIPRange = false;
                    break;
                case "Суперпользователь":
                    HasAccessToUsers = true;
                    CanEditDevices = true;
                    CanUpdateIPRange = true;
                    break;
                default:
                    throw new InvalidOperationException("User has role that is not registered");
            }
        }
    }
}
