using System;
using System.Collections.Generic;
using DevSpector.SDK.Models;
using DevSpector.SDK.DTO;

namespace DevSpector.Desktop.Service
{
    public interface IDevicesStorage
    {
        IList<Device> GetDevicesAsync();

        void AddDeviceAsync(DeviceToCreate deviceInfo);

        void RemoveDeviceAsync(string inventoryNumber);

        void UpdateDeviceAsync(DeviceToCreate deviceInfo);
    }
}
