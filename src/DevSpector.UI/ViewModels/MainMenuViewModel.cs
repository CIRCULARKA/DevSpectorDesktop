using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using DevSpector.Desktop.UI.Views;
using DevSpector.Desktop.Service;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class MainMenuViewModel : ViewModelBase, IMainMenuViewModel
    {
        private UserControl _currentContent;

        public MainMenuViewModel(
            UsersMainView usersMainView,
            IUserRights userRights
        ) : base(userRights)
        {
            UsersMainView = usersMainView;

            ChangeContentCommand = ReactiveCommand.Create<string>(
                ChangeContent
            );
        }

        public ReactiveCommand<string, Unit> ChangeContentCommand { get; }

        public void ChangeContent(string viewName)
        {
            switch (viewName)
            {
                case nameof(DevicesMainView):
                    CurrentContent = DevicesMainView;
                    break;
                case nameof(UsersMainView):
                    CurrentContent = UsersMainView;
                    break;
            }
        }

        public UserControl CurrentContent
        {
            get => _currentContent;
            set { this.RaiseAndSetIfChanged(ref _currentContent, value); }
        }

        public UserControl DevicesMainView { get; }

        public UserControl UsersMainView { get; }
    }
}
