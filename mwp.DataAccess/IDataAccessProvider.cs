using mwp.DataAccess.Entities;
using System.Threading.Tasks;

namespace mwp.DataAccess
{
    public interface IDataAccessProvider
    {
        Task AddUser(User user);
        Task UpdateUser(long userId, User user);
        Task DeleteUser(long userId);
        Task<User> GetUser(long userId);
        Task<bool> UserExists(long userId);
        Task AddUserGroup(UserGroup userGroup);
    }
}