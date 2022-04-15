using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public interface IUsersStorage
    {
        Task<List<User>> GetUsersAsync();

        Task<List<UserGroup>> GetUserGroupsAsync();

        Task AddUserAsync(UserToCreate userInfo);

        Task UpdateUserAsync(string targetLogin, UserToCreate userInfo);

        Task RemoveUserAsync(string login);

        Task<string> RevokeAccessKeyAsync(string login, string password);

        Task ChangePasswordAsync(string login, string oldPassword, string newPassword);
    }
}
