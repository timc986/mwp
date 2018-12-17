using System.Threading.Tasks;
using mwp.DataAccess.Entities;
using mwp.Service.Repository;
using Microsoft.EntityFrameworkCore;

namespace mwp.Service.UnitOfWork
{
    public interface IUnitOfWork
    {
        DbContext DatabaseContext { get; }
        Task<bool> Save();

        IBaseRepository<User> UserRepository { get; }
        IBaseRepository<Record> RecordRepository { get; }
    }
}
