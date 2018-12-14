using mwp.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using mwp.DataAccess.Entities;

namespace mwp.Service.Login
{
    public class LoginService: ILoginService
    {
        private readonly IDataAccessProvider dataAccessProvider;

        public LoginService(IDataAccessProvider dataAccessProvider)
        {
            this.dataAccessProvider = dataAccessProvider;
        }

        public async Task<bool> CheckUserExist(long id)
        {
            try
            {
                var result = await dataAccessProvider.UserExists(id);

                return result;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }

        public async Task<User> GetUser(long id)
        {
            try
            {
                var result = await dataAccessProvider.GetUser(id);

                return result;
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public async Task<bool> CreateUser(User user)
        {
            try
            {
                //var newUserGroup = new UserGroup {Name = "TimGroup", CreatedOn = DateTime.UtcNow};

                //await dataAccessProvider.AddUserGroup(newUserGroup);


                await dataAccessProvider.AddUser(user);

                return true;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }
    }
}
