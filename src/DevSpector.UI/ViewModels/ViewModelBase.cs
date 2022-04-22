using ReactiveUI;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _canEditDevices;

        private bool _hasAccessToUsers;

        private bool _canUpdateIPRange;

        private readonly IUserRights _userRights;

        public ViewModelBase(IUserRights userRights)
        {
            _userRights = userRights;
        }

        public bool CanEditDevices
        {
            get => _canEditDevices;
            set => this.RaiseAndSetIfChanged(ref _canEditDevices, value);
        }

        public bool HasAccessToUsers
        {
            get => _hasAccessToUsers;
            set => this.RaiseAndSetIfChanged(ref _hasAccessToUsers, value);
        }

        public bool CanUpdateIPRange
        {
            get => _canUpdateIPRange;
            set => this.RaiseAndSetIfChanged(ref _canUpdateIPRange, value);
        }

        public void UpdateUserRights(User u)
        {
            _userRights.SetUser(u);

            CanEditDevices = _userRights.CanEditDevices;
            HasAccessToUsers = _userRights.HasAccessToUsers;
            CanUpdateIPRange = _userRights.CanUpdateIPRange;
        }
    }
}
