using mwp.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace mwp.Service.Login
{
    public class LoginService: ILoginService
    {
        private readonly IDataAccessProvider dataAccessProvider;

        public LoginService(IDataAccessProvider dataAccessProvider)
        {
            this.dataAccessProvider = dataAccessProvider;
        }

        public bool CheckUserExist(long id)
        {
            try
            {
                var result = false;

                var xx = dataAccessProvider.UserExists(id);

                return result;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }
    }
}
