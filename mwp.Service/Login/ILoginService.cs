using System;
using System.Collections.Generic;
using System.Text;

namespace mwp.Service.Login
{
    public interface ILoginService
    {
        bool CheckUserExist(long id);
    }
}
