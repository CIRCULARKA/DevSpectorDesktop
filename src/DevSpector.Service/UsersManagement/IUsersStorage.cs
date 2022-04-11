using System.Threading.Tasks;
using System.Collections.Generic;
using DevSpector.SDK.DTO;
using DevSpector.SDK.Models;

namespace DevSpector.Desktop.Service
{
    public interface IUsersStorage
    {
        public Task<List<UserGroup>> GetUserGroupsAsync();

        public Task AddUserAsync(UserToCreate userInfo);

        public Task UpdateUserAsync(string targetLogin, UserToCreate userInfo);

        public Task RemoveUserAsync(string login);
    }
}
