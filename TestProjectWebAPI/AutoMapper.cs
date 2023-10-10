using AutoMapper;
using DocumentService.Entities;
using DocumentService.Models.dto;

namespace DocumentService
{
    public class AutoMapper : Profile
    {
        public AutoMapper() 
        {
            CreateMap<UserEntity, UserInfoDto>();
            CreateMap<RoleEntity, RoleDto>();
            CreateMap<UserEntity, RegisterUserDto>();
            CreateMap<UserEntity, UpdateUserDataDto>();
        }
    }
}
