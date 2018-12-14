using System.Threading.Tasks;
using mwp.DataAccess.Entities;

namespace mwp.Service
{
    public interface IUserService
    {
        Task<bool> CheckUserExist(long id);
        Task<DataAccess.Entities.User> GetUser(long id);
        Task<bool> CreateUser(User user);
        Task<User> Login(string username, string password);
        Task<User> Create(User user, string password);
    }
}
