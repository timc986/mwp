using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;
using mwp.Service.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace mwp.Service.Service
{
    public class RecordService : IRecordService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RecordService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Record> GetRecord(long id)
        {
            var result = await unitOfWork.RecordRepository.GetFirstOrDefault(r => r.Id == id);

            return result;
        }

        public async Task<List<RecordDto>> GetUserRecord(string userIdString)
        {
            var userId = Convert.ToInt64(userIdString);
            var records = await unitOfWork.RecordRepository.SearchBy(r => r.UserId == userId).ToListAsync();

            var recordDtos = mapper.Map<List<RecordDto>>(records);

            return recordDtos;
        }

        public async Task<RecordDto> CreateRecord(RecordDto createRecord, string userId)
        {
            var record = mapper.Map<Record>(createRecord);
            record.UserId = Convert.ToInt64(userId);

            await unitOfWork.RecordRepository.Add(record);
            await unitOfWork.Save();

            var recordDto = mapper.Map<RecordDto>(record);

            return recordDto;
        }
    }
}
