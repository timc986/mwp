using System.Threading.Tasks;
using mwp.DataAccess.Dto;

namespace mwp.Service.Service
{
    public interface IUserService
    {
        Task<UserDto> GetUser(long id);
        Task<UserDto> Login(string username, string password);
        Task<UserDto> Create(UserDto createUser);
    }
}
