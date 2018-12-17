using System.Threading.Tasks;
using mwp.DataAccess.Entities;

namespace mwp.Service
{
    public interface IUserService
    {
        Task<DataAccess.Entities.User> GetUser(long id);
        Task<User> Login(string username, string password);
        Task<User> Create(User user, string password);
    }
}
