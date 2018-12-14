using mwp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mwp.Service.Login
{
    public interface ILoginService
    {
        Task<bool> CheckUserExist(long id);
        Task<User> GetUser(long id);
        Task<bool> CreateUser(User user);
    }
}
