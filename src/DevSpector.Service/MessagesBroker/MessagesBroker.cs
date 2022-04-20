using System;

namespace DevSpector.Desktop.Service
{
    public class MessagesBroker : IMessagesBroker
    {
        private readonly ApplicationEvents _appEvents;

        public MessagesBroker(ApplicationEvents events)
        {
            _appEvents = events;
        }

        public void NotifyUser(string message)
        {
            _appEvents.RaiseUserNotified(PrependCurrentTime(message));
        }

        private string PrependCurrentTime(string message)
        {
            return $"[{DateTime.Now.ToLongTimeString()}]: {message}";
        }
    }
}
