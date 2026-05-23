using AutoMapper;
using backend.DTOs;
using backend.Models;

namespace backend.Mapping
{
    public class BarberProfile : Profile
    {
        public BarberProfile()
        {
            CreateMap<Barber, BarberDto>().ReverseMap();
        }
    }
}