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

        public async Task<List<Device>> GetDevicesAsync()
        {
            var issueMessage = "Устройства не могут быть получены";

            List<Device> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _provider.GetDevicesAsync(),
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
                async () => result = await _provider.GetDeviceTypesAsync(),
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
