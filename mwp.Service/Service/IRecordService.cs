﻿using System.Threading.Tasks;
using mwp.DataAccess.Entities;

namespace mwp.Service.Service
{
    public interface IRecordService
    {
        Task<Record> GetRecord(long id);
        Task<Record> GetUserRecord(long userId);
        Task<Record> CreateRecord(Record record);
    }
}