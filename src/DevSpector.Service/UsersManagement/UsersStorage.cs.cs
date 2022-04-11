using System.Threading.Tasks;
using DevSpector.SDK.DTO;
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
