using System;
using System.Net.Http;
using System.Threading.Tasks;
using DevSpector.SDK.Exceptions;

namespace DevSpector.Desktop.Service
{
    public class StorageBase
    {
        protected async Task ReThrowExceptionFrom(
            Func<Task> action,
            string issueMessage
        )
        {
            try
            {
                await action();
            }
            catch (UnauthorizedException)
            {
                throw new InvalidOperationException($"{issueMessage} - нет доступа");
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"{issueMessage} - неизвестная ошибка");
            }
            catch (HttpRequestException)
            {
                throw new InvalidOperationException($"{issueMessage} - нет связи с сервером");
            }
        }
    }
}
