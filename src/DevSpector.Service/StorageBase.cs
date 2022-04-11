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
            string issueMessage,
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
                throw new InvalidOperationException($"{issueMessage} - {noAccessMessage}");
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException($"{issueMessage} - {unhandledMessage}");
            }
            catch (HttpRequestException)
            {
                throw new InvalidOperationException($"{noConnectionMessage} - {unhandledMessage}");
            }
    }
}
