using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.SDK.Editors;
using DevSpector.SDK.Providers;
using DevSpector.SDK.Networking;

namespace DevSpector.Desktop.Service
{
    public class DevicesStorage : StorageBase, IDevicesStorage
    {
        private readonly IDevicesEditor _editor;

        private readonly IDevicesProvider _devicesProvider;

        private readonly ILocationProvider _locationProvider;

        private readonly INetworkManager _networkManager;

        private readonly IApplicationEvents _appEvents;

        public DevicesStorage(
            IDevicesEditor editor,
            IDevicesProvider devicesProvider,
            ILocationProvider locationProvider,
            INetworkManager networkManager,
            IApplicationEvents appEvents
        )
        {
            _editor = editor;
            _devicesProvider = devicesProvider;
            _locationProvider = locationProvider;
            _networkManager = networkManager;

            _appEvents = appEvents;
        }

        public async Task<List<Device>> GetDevicesAsync()
        {
            List<Device> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _devicesProvider.GetDevicesAsync(),
                "Устройства не могут быть получены"
            );

            return result;
        }

        public async Task<List<DeviceType>> GetDevicesTypesAsync()
        {

            List<DeviceType> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _devicesProvider.GetDeviceTypesAsync(),
                "Не удалось получить доступные типы устройств"
            );

            return result;
        }

        public async Task<List<Cabinet>> GetCabinetsAsync(string housingID)
        {
            List<Cabinet> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _locationProvider.GetHousingCabinetsAsync(housingID),
                "Не удалось загрузить список корпусов"
            );

            return result;
        }

        public async Task<List<Housing>> GetHousingsAsync()
        {
            List<Housing> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _locationProvider.GetHousingsAsync(),
                "Не удалось загрузить коппусы"
            );

            return result;
        }

        public async Task<List<string>> GetFreeIPAsync()
        {
            List<string> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _networkManager.GetFreeIPAsync(),
                "Не удалось загрузить свободные IP-адреса"
            );

            return result;
        }

        public async Task AddDeviceAsync(DeviceToCreate deviceInfo)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.CreateDeviceAsync(deviceInfo),
                "Устройство не может быть добавлено"
            );
        }

        public async Task RemoveDeviceAsync(string inventoryNumber)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.DeleteDeviceAsync(inventoryNumber),
                "Устройство не может быть удалено"
            );
        }

        public async Task UpdateDeviceAsync(string targetInventoryNumber, DeviceToCreate deviceInfo)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.UpdateDeviceAsync(targetInventoryNumber, deviceInfo),
                "Устройство не может быть обновлено"
            );
        }

        public async Task MoveDeviceAsync(string inventoryNumber, string cabinetID)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.MoveAsync(inventoryNumber, cabinetID),
                "Устройство не может быть перемещено"
            );
        }

        public async Task AddSoftwareAsync(string inventoryNumber, Software software)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.AddSoftwareAsync(inventoryNumber, software),
                "Не удалось добавить ПО к устройству"
            );
        }

        public async Task RemoveSoftwareAsync(string inventoryNumber, Software software)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.RemoveSoftwareAsync(inventoryNumber, software),
                "Не удалось удалить ПО"
            );
        }

        public async Task AddIPAsync(string inventoryNumber, string ip)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.AssignIPAsync(inventoryNumber, ip),
                "Не удалось добавить IP к устройству"
            );
        }

        public async Task RemoveIPAsync(string inventoryNumber, string ip)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.RemoveIPAsync(inventoryNumber, ip),
                "Не удалось удалить IP с устройства"
            );
        }

        public async Task UpdateIPRangeAsync(int mask, string networkAddress)
        {
            await ReThrowExceptionFrom(
                async () => await _networkManager.GenerateIPRangeAsync(networkAddress, mask),
                "Не удалось обновить диапазон IP-адресов"
            );

            _appEvents.RaiseIPRangeUpdated();
        }
    }
}
