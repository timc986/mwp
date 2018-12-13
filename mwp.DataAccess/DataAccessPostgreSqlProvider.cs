using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mwp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace mwp.DataAccess
{
    public class DataAccessPostgreSqlProvider: IDataAccessProvider
    {
        private readonly DomainModelPostgreSqlContext context;

        public DataAccessPostgreSqlProvider(DomainModelPostgreSqlContext context)
        {
            this.context = context;
        }

        public async Task AddUser(User user)
        {
            context.User.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateUser(long userId, User user)
        {
            context.User.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUser(long userId)
        {
            var entity = context.User.First(u => u.Id == userId);
            context.User.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<User> GetUser(long userId)
        {
            return await context.User.FirstAsync(u => u.Id == userId);
        }

        public async Task<bool> UserExists(long userId)
        {
            var user = context.User.Where(u => u.Id == userId);

            return await user.AnyAsync();
        }
    }
}
