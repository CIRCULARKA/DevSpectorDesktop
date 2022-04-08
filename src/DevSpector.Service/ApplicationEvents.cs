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

        public event Action Logout;

        public void RaiseLogout() =>
            Logout?.Invoke();

        public event Action<string> UserNotified;

        public void RaiseUserNotified(string message) =>
            UserNotified?.Invoke(message);
    }
}
