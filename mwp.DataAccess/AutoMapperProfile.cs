using AutoMapper;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;

namespace mwp.DataAccess
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LoginRequest, User>();
            CreateMap<CreateUserRequest, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, LoginResponse>();
            CreateMap<CreateRecordRequest, Record>();
            CreateMap<Record, RecordDto>();
            CreateMap<RecordDto, Record>();
        }
    }
}
