using ReactiveUI;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class MessagesBrokerViewModel : ViewModelBase, IMessagesBrokerViewModel
    {
        private string _message;

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }
    }
}
