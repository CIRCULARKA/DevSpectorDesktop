using System;
using System.Collections.Generic;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public class ApplicationEvents : IApplicationEvents
    {
        public ApplicationEvents() { }

        public event Action<Device> DeviceSelected;

        public void RaiseDeviceSelected(Device Device) =>
            DeviceSelected?.Invoke(Device);

        public event Action DeviceUpdated;

        public void RaiseDeviceUpdated() =>
            DeviceUpdated?.Invoke();

        public event Action<Device> DeviceDeleted;

        public void RaiseDeviceDeleted(Device device) =>
            DeviceDeleted?.Invoke(device);

        public event Action<IEnumerable<Device>> SearchExecuted;

        public void RaiseSearchExecuted(IEnumerable<Device> filtered) =>
            SearchExecuted?.Invoke(filtered);

        public event Action<User> UserAuthorized;

        public void RaiseUserAuthorized(User user) =>
            UserAuthorized?.Invoke(user);

        public event Action AuthorizationCompleted;

        public void RaiseAuthorizationCompleted() =>
            AuthorizationCompleted?.Invoke();

        public event Action<User> UserSelected;

        public void RaiseUserSelected(User user) =>
            UserSelected?.Invoke(user);

        public event Action UserCreated;

        public void RaiseUserCreated() =>
            UserCreated?.Invoke();

        public event Action UserRemoved;

        public void RaiseUserRemoved() =>
            UserRemoved?.Invoke();

        public event Action UserUpdated;

        public void RaiseUserUpdated() =>
            UserUpdated?.Invoke();

        public event Action Logout;

        public void RaiseLogout() =>
            Logout?.Invoke();

        public event Action<string> UserNotified;

        public void RaiseUserNotified(string message) =>
            UserNotified?.Invoke(message);

        public event Action<Device, string> IPAddressDeleted;

        public void RaiseOnIPAddressDeleted(Device device, string ip) =>
            IPAddressDeleted?.Invoke(device, ip);

        public event Action<Device, string> IPAddressAdded;

        public void RaiseOnIPAddressAdded(Device device, string ip) =>
            IPAddressAdded?.Invoke(device, ip);

        public event Action IPRangeUpdated;

        public void RaiseIPRangeUpdated() =>
            IPRangeUpdated?.Invoke();
    }
}
