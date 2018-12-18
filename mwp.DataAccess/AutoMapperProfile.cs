using AutoMapper;
using mwp.DataAccess.Dto;
using mwp.DataAccess.Entities;

namespace mwp.DataAccess
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
