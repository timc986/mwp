using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using mwp.DataAccess.Entities;
using mwp.Service.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace mwp.Service.Service
{
    public class RecordService : IRecordService
    {
        private readonly IUnitOfWork unitOfWork;

        public RecordService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Record> GetRecord(long id)
        {
            try
            {
                var result = await unitOfWork.RecordRepository.GetFirstOrDefault(r => r.Id == id);

                return result;
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public async Task<List<Record>> GetUserRecord(long userId)
        {
            try
            {
                var result = await unitOfWork.RecordRepository.SearchBy(r => r.UserId == userId).ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public async Task<Record> CreateRecord(Record record)
        {
            try
            {
                await unitOfWork.RecordRepository.Add(record);

                await unitOfWork.Save();

                return record;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
