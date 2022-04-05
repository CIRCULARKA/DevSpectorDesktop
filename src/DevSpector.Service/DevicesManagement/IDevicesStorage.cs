using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.Models;
using DevSpector.SDK.DTO;

namespace DevSpector.Desktop.Service
{
    public interface IDevicesStorage
    {
        Task<IList<Device>> GetDevicesAsync();

        Task<IList<DeviceType>> GetDevicesTypesAsync();

        Task AddDeviceAsync(DeviceToCreate deviceInfo);

        Task RemoveDeviceAsync(string inventoryNumber);

        Task UpdateDeviceAsync(string targetInventoryNumber, DeviceToCreate deviceInfo);
    }
}
