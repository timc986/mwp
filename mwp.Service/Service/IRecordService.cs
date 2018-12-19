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
        Task<RecordDto> UpdateRecord(RecordDto updateRecord, string userId);
        Task<RecordDto> UpdateRecordVisibility(long recordId, long visibilityId, string userId);
        Task DeleteRecord(long recordId, string userId);
    }
}