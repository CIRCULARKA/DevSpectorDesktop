using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.Models;
using DevSpector.SDK.DTO;

namespace DevSpector.Desktop.Service
{
    public interface IDevicesStorage
    {
        Task<List<Device>> GetDevicesAsync();

        Task<List<DeviceType>> GetDevicesTypesAsync();

        Task<List<Housing>> GetHousingsAsync();

        Task<List<Cabinet>> GetCabinetsAsync(string housingID);

        Task AddDeviceAsync(DeviceToCreate deviceInfo);

        Task RemoveDeviceAsync(string inventoryNumber);

        Task UpdateDeviceAsync(string targetInventoryNumber, DeviceToCreate deviceInfo);

        Task MoveDeviceAsync(string inventoryNumber, string cabinetID);

        Task AddSoftwareAsync(string inventoryNumber, Software software);

        Task RemoveSoftwareAsync(string inventoryNumber, Software software);
    }
}
