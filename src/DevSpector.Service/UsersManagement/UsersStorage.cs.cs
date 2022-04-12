using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;
using DevSpector.SDK.Editors;
using DevSpector.SDK.Providers;

namespace DevSpector.Desktop.Service
{
    public class UsersStorage : StorageBase, IUsersStorage
    {
        private readonly IUsersEditor _usersEditor;

        private readonly IUsersProvider _usersProvider;

        private readonly IApplicationEvents _appEvents;

        public UsersStorage(
            IUsersProvider provider,
            IUsersEditor editor,
            IApplicationEvents appEvents
        )
        {
            _usersEditor = editor;
            _usersProvider = provider;
            _appEvents = appEvents;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users = null;

            await ReThrowExceptionFrom(
                async () => users = await _usersProvider.GetUsersAsync(),
                "Не удалось получить группы пользователей"
            );

            return users;
        }

        public async Task<List<UserGroup>> GetUserGroupsAsync()
        {
            List<UserGroup> result = null;

            await ReThrowExceptionFrom(
                async () => result = await _usersProvider.GetUserGroupsAsync(),
                "Не удалось получить группы пользователей"
            );

            return result;
        }

        public async Task AddUserAsync(UserToCreate userInfo)
        {
            await ReThrowExceptionFrom(
                async () => await _usersEditor.CreateUserAsync(userInfo),
                "Не удалось добавить пользователя"
            );

            _appEvents.RaiseUserCreated();
        }

        public async Task UpdateUserAsync(string targetLogin, UserToCreate userInfo)
        {
            await ReThrowExceptionFrom(
                async () => await _usersEditor.UpdateUserAsync(targetLogin, userInfo),
                "Не удалось добавить пользователя"
            );

            _appEvents.RaiseUserUpdated();
        }

        public async Task RemoveUserAsync(string login)
        {
            await ReThrowExceptionFrom(
                async () => await _usersEditor.DeleteUserAsync(login),
                "Не удалось добавить пользователя"
            );

            _appEvents.RaiseUserRemoved();
        }
    }
}
