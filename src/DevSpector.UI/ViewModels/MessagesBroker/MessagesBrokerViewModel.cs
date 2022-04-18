using ReactiveUI;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class MessagesBrokerViewModel : ViewModelBase, IMessagesBrokerViewModel
    {
        public MessagesBrokerViewModel(
            IUserRights userRights
        ) : base(userRights) { }

        private string _message;

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public void ClearMessages()
        {
            Message = string.Empty;
        }
    }
}
