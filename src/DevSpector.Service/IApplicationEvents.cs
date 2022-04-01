using System;
using System.Collections.Generic;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public interface IApplicationEvents
    {
        event Action<Device> DeviceSelected;

        void RaiseDeviceSelected(Device Device);

        event Action<IEnumerable<Device>> SearchExecuted;

        void RaiseSearchExecuted(IEnumerable<Device> filtered);

        event Action<User> UserAuthorized;

        void RaiseUserAuthorized(User user);

        event Action AuthorizationCompleted;

        void RaiseAuthorizationCompleted();

        event Action<User> UserSelected;

        void RaiseUserSelected(User user);

        event Action Logout;

        void RaiseLogout();
    }
}
