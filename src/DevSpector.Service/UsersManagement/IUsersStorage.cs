using System.Threading.Tasks;
using DevSpector.SDK.DTO;

namespace DevSpector.Desktop.Service
{
    public interface IUsersStorage
    {
        public Task AddUserAsync(UserToCreate userInfo);

        public Task UpdateUserAsync(UserToCreate userInfo);

        public Task RemoveUserAsync(string login);
    }
}
