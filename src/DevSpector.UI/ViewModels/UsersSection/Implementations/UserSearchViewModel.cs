using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ReactiveUI;
using DevSpector.Desktop.Service;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.UI.ViewModels
{
    public class UserSearchViewModel : ViewModelBase, IUserSearchViewModel
    {
        private string _searchQuery;

        private ApplicationEvents _events;

        private IUsersListViewModel _usersListViewModel;

        public UserSearchViewModel(
            ApplicationEvents events,
            IUsersListViewModel usersListViewModel,
            IUserRights userRights
        ) : base(userRights)
        {
            _events = events;
            _usersListViewModel = usersListViewModel;

            SearchCommand = ReactiveCommand.CreateFromTask(
                async () => {
                    try
                    {
                        _usersListViewModel.AreItemsLoaded = false;
                        _usersListViewModel.AreThereItems = false;
                        events.RaiseUserSearched(
                            await FilterUsersAsync(_usersListViewModel.ItemsCache)
                        );
                    }
                   finally { _usersListViewModel.AreItemsLoaded = true; }
                }
            );
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
        }

        public ReactiveCommand<Unit, Unit> SearchCommand { get; }

        private Task<IEnumerable<User>> FilterUsersAsync(IEnumerable<User> users)
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                    return Task.FromResult(new List<User>(users).AsEnumerable());

            var result = new List<User>(users.Count());

            var filteringTask = Task.Run(
                () => {
                    result.AddRange(users.Where(d => d.Login.Contains(SearchQuery)));
                    result.AddRange(users.Where(d => d.FirstName.Contains(SearchQuery)));
                    result.AddRange(users.Where(d => d.Surname.Contains(SearchQuery)));
                    result.AddRange(users.Where(d => d.Patronymic.Contains(SearchQuery)));
                    result.AddRange(users.Where(d => d.Group.Contains(SearchQuery)));

                    Thread.Sleep(3000);

                    return result.AsEnumerable<User>();
                }
            );

            return filteringTask;
        }
    }
}
