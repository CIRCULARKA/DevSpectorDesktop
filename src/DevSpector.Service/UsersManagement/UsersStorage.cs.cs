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

        public UsersStorage(
            IUsersProvider provider,
            IUsersEditor editor
        )
        {
            _usersEditor = editor;
            _usersProvider = provider;
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
        }

        public async Task UpdateUserAsync(string targetLogin, UserToCreate userInfo)
        {
            await ReThrowExceptionFrom(
                async () => await _usersEditor.UpdateUserAsync(targetLogin, userInfo),
                "Не удалось добавить пользователя"
            );
        }

        public async Task RemoveUserAsync(string login)
        {
            await ReThrowExceptionFrom(
                async () => await _usersEditor.DeleteUserAsync(login),
                "Не удалось добавить пользователя"
            );
        }
    }
}
