using System;
using System.Threading.Tasks;
using mwp.DataAccess.Entities;
using mwp.Service.Repository;
using Microsoft.EntityFrameworkCore;

namespace mwp.Service.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext DatabaseContext { get; private set; }

        private IBaseRepository<User> userRepository;
        private IBaseRepository<Record> recordRepository;

        public UnitOfWork(DbContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        public async Task<bool> Save()
        {
            try
            {
                int save = await DatabaseContext.SaveChangesAsync();
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                throw;
                //return await Task.FromResult(false);
            }
        }

        public IBaseRepository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new BaseRepository<User>(DatabaseContext);
                }
                return userRepository;
            }
        }

        public IBaseRepository<Record> RecordRepository
        {
            get
            {
                if (recordRepository == null)
                {
                    recordRepository = new BaseRepository<Record>(DatabaseContext);
                }
                return recordRepository;
            }
        }
    }
}
