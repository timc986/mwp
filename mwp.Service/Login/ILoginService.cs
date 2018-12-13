using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mwp.Service.Login
{
    public interface ILoginService
    {
        Task<bool> CheckUserExist(long id);
    }
}
