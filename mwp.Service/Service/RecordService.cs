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

        public async Task<RecordDto> UpdateRecord(RecordDto updateRecord, string userId)
        {
            //Only allows the same user to update its own record
            var existingRecord = await unitOfWork.RecordRepository.GetFirstOrDefault(r => r.Id == updateRecord.Id && r.UserId == Convert.ToInt64(userId));
            if (existingRecord == null)
            {
                throw new Exception("Invalid request");
            }

            //Only update these fields
            existingRecord.Content = updateRecord.Content;
            existingRecord.Title = updateRecord.Title;
            if (updateRecord.RecordVisibilityId > 0)
            {
                existingRecord.RecordVisibilityId = updateRecord.RecordVisibilityId;
            }

            await unitOfWork.RecordRepository.Update(existingRecord);
            await unitOfWork.Save();

            var recordDto = mapper.Map<RecordDto>(existingRecord);

            return recordDto;
        }

        public async Task<RecordDto> UpdateRecordVisibility(long recordId, long visibilityId, string userId)
        {
            //Only allows the same user to update its own record
            var existingRecord = await unitOfWork.RecordRepository.GetFirstOrDefault(r => r.Id == recordId && r.UserId == Convert.ToInt64(userId));
            if (existingRecord == null)
            {
                throw new Exception("Invalid request");
            }

            //Only update this fields
            if (visibilityId < 1)
            {
                throw new Exception("Invalid visibility Id");
            }

            existingRecord.RecordVisibilityId = visibilityId;

            await unitOfWork.RecordRepository.Update(existingRecord);
            await unitOfWork.Save();

            var recordDto = mapper.Map<RecordDto>(existingRecord);

            return recordDto;
        }

        public async Task DeleteRecord(long recordId, string userId)
        {
            //Only allows the same user to delete its own record
            var existingRecord = await unitOfWork.RecordRepository.GetFirstOrDefault(r => r.Id == recordId && r.UserId == Convert.ToInt64(userId));
            if (existingRecord == null)
            {
                throw new Exception("Invalid request");
            }

            await unitOfWork.RecordRepository.Delete(r => r.Id == recordId && r.UserId == Convert.ToInt64(userId));
            await unitOfWork.Save();
        }
    }
}
