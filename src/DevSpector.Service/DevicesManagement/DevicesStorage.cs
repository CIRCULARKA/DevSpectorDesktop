using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.SDK.Editors;
using DevSpector.SDK.Providers;
using DevSpector.SDK.Exceptions;

namespace DevSpector.Desktop.Service
{
    public class DevicesStorage : IDevicesStorage
    {
        private readonly IDevicesEditor _editor;

        private readonly IDevicesProvider _provider;

        public DevicesStorage(IDevicesEditor editor, IDevicesProvider provider)
        {
            _editor = editor;
            _provider = provider;
        }

        public async Task<IList<Device>> GetDevicesAsync()
        {
            List<Device> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _provider.GetDevicesAsync(),
                "Устройства не могут быть получены - нет доступа",
                "Устройства не могут быть получены - нет связи с сервером",
                "Устройства не могут быть получены - неизвестная ошибка"
            );

            return result;
        }

        public async Task AddDeviceAsync(DeviceToCreate deviceInfo)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.CreateDeviceAsync(deviceInfo),
                "Устройство не может быть добавлено - нет доступа",
                "Устройство не может быть добавлено - нет связи с сервером",
                "Устройство не может быть добавлено - неизвестная ошибка"
            );
        }

        public async Task RemoveDeviceAsync(string inventoryNumber)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.DeleteDeviceAsync(inventoryNumber),
                "Устройство не может быть удалено - нет доступа",
                "Устройство не может быть удалено - нет связи с сервером",
                "Устройство не может быть удалено - неизвестная ошибка"
            );
        }

        public async Task UpdateDeviceAsync(string targetInventoryNumber, DeviceToCreate deviceInfo)
        {
            await ReThrowExceptionFrom(
                async () => await _editor.UpdateDeviceAsync(targetInventoryNumber, deviceInfo),
                "Устройство не может быть удалено - нет доступа",
                "Устройство не может быть удалено - нет связи с сервером",
                "Устройство не может быть удалено - неизвестная ошибка"
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
