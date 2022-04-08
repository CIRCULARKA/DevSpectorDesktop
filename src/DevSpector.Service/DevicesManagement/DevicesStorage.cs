using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.SDK.Editors;
using DevSpector.SDK.Providers;
using DevSpector.SDK.Networking;
using DevSpector.SDK.Exceptions;

namespace DevSpector.Desktop.Service
{
    public class DevicesStorage : IDevicesStorage
    {
        private readonly IDevicesEditor _editor;

        private readonly IDevicesProvider _devicesProvider;

        private readonly ILocationProvider _locationProvider;

        private readonly INetworkManager _networkManager;

        public DevicesStorage(
            IDevicesEditor editor,
            IDevicesProvider devicesProvider,
            ILocationProvider locationProvider,
            INetworkManager networkManager
        )
        {
            _editor = editor;
            _devicesProvider = devicesProvider;
            _locationProvider = locationProvider;
            _networkManager = networkManager;
        }

        public async Task<List<Device>> GetDevicesAsync()
        {
            var issueMessage = "Устройства не могут быть получены";

            List<Device> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _devicesProvider.GetDevicesAsync(),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );

            return result;
        }

        public async Task<List<DeviceType>> GetDevicesTypesAsync()
        {
            var issueMessage = "Типы устройств не могут быть получены";

            List<DeviceType> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _devicesProvider.GetDeviceTypesAsync(),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );

            return result;
        }

        public async Task<List<Cabinet>> GetCabinetsAsync(string housingID)
        {
            var issueMessage = "Не удалось загрузить список корпусов";

            List<Cabinet> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _locationProvider.GetHousingCabinetsAsync(housingID),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );

            return result;
        }

        public async Task<List<Housing>> GetHousingsAsync()
        {
            var issueMessage = "Не удалось загрузить коппусы";

            List<Housing> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _locationProvider.GetHousingsAsync(),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );

            return result;
        }

        public async Task<List<string>> GetFreeIP()
        {
            var issueMessage = "Не удалось загрузить свободные IP-адреса";

            List<string> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _networkManager.GetFreeIPAsync(),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );

            return result;
        }

        public async Task AddDeviceAsync(DeviceToCreate deviceInfo)
        {
            var issueMessage = "Устройство не может быть добавлено";

            await ReThrowExceptionFrom(
                async () => await _editor.CreateDeviceAsync(deviceInfo),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );
        }

        public async Task RemoveDeviceAsync(string inventoryNumber)
        {
            var issueMessage = "Устройство не может быть удалено";

            await ReThrowExceptionFrom(
                async () => await _editor.DeleteDeviceAsync(inventoryNumber),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );
        }

        public async Task UpdateDeviceAsync(string targetInventoryNumber, DeviceToCreate deviceInfo)
        {
            var issueMessage = "Устройство не может быть обновлено";

            await ReThrowExceptionFrom(
                async () => await _editor.UpdateDeviceAsync(targetInventoryNumber, deviceInfo),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );
        }

        public async Task MoveDeviceAsync(string inventoryNumber, string cabinetID)
        {
            var issueMessage = "Устройство не может быть перемещено";

            await ReThrowExceptionFrom(
                async () => await _editor.MoveAsync(inventoryNumber, cabinetID),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );
        }

        public async Task AddSoftwareAsync(string inventoryNumber, Software software)
        {
            var issueMessage = "Не удалось добавить ПО к устройству";

            await ReThrowExceptionFrom(
                async () => await _editor.AddSoftwareAsync(inventoryNumber, software),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );
        }

        public async Task RemoveSoftwareAsync(string inventoryNumber, Software software)
        {
            var issueMessage = "Не удалось удалить ПО";

            await ReThrowExceptionFrom(
                async () => await _editor.RemoveSoftwareAsync(inventoryNumber, software),
                $"{issueMessage} - нет доступа",
                $"{issueMessage} - нет связи с сервером",
                $"{issueMessage} - неизвестная ошибка"
            );
        }

        private async Task ReThrowExceptionFrom(
            Func<Task> action,
            string noAccessMessage,
            string noConnectionMessage,
            string unhandledMessage
        )
        {
            try
            {
                await action();
            }
            catch (UnauthorizedException)
            {
                throw new InvalidOperationException(noAccessMessage);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException(unhandledMessage);
            }
            catch (HttpRequestException)
            {
                throw new InvalidOperationException(noConnectionMessage);
            }
        }
    }
}
