using System.Collections.Generic;
using System.Threading.Tasks;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;

namespace mwp.Service.Service
{
    public interface IRecordService
    {
        Task<Record> GetRecord(long id);
        Task<List<RecordDto>> GetUserRecord(string userIdString);
        Task<RecordDto> CreateRecord(RecordDto createRecord, string userId);
    }
}