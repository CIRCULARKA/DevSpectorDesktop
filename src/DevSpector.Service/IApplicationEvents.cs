using System;
using System.Collections.Generic;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public interface IApplicationEvents
    {
        event Action<Device> DeviceSelected;

        void RaiseDeviceSelected(Device Device);

        event Action DeviceUpdated;

        void RaiseDeviceUpdated();

        event Action<Device> DeviceDeleted;

        void RaiseDeviceDeleted(Device device);

        event Action<IEnumerable<Device>> SearchExecuted;

        void RaiseSearchExecuted(IEnumerable<Device> filtered);

        event Action<User> UserAuthorized;

        void RaiseUserAuthorized(User user);

        event Action<User> UserSelected;

        void RaiseUserSelected(User user);

        event Action UserCreated;

        void RaiseUserCreated();

        event Action UserRemoved;

        void RaiseUserRemoved();

        event Action UserUpdated;

        void RaiseUserUpdated();

        event Action Logout;

        void RaiseLogout();

        event Action<string> UserNotified;

        void RaiseUserNotified(string message);

        event Action<Device, string> IPAddressDeleted;

        void RaiseOnIPAddressDeleted(Device device, string ip);

        event Action IPRangeUpdated;

        void RaiseIPRangeUpdated();

        event Action<Device, string> IPAddressAdded;

        void RaiseOnIPAddressAdded(Device device, string ip);
    }
}
