namespace DevSpector.Desktop.Service
{
    public class MessagesBroker : IMessagesBroker
    {
        private readonly IApplicationEvents _appEvents;

        public MessagesBroker(IApplicationEvents events)
        {
            _appEvents = events;
        }

        public void NotifyUser(string message)
        {
            _appEvents.RaiseUserNotified(message);
        }
    }
}
