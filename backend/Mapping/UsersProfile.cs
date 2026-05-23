using AutoMapper;
using backend.Data;
using backend.DTOs;

namespace backend.Mapping
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName));
        }       
    }
}