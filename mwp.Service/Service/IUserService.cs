using System.Threading.Tasks;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;

namespace mwp.Service.Service
{
    public interface IUserService
    {
        Task<DataAccess.Entities.User> GetUser(long id);
        Task<UserDto> Login(string username, string password);
        Task<User> Create(User user, string password);
    }
}
