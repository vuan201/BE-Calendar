
using Application.DTOs.AuthDTO;
using Application.DTOs.EventDTO;
using Application.DTOs.UserDTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDTO>().ReverseMap();
        CreateMap<Event, CreateEventDTO>().ReverseMap();
        CreateMap<ApplicationUser, UserInfomationDTO>().ReverseMap();
        CreateMap<ApplicationUser, AuthRegisterDTO>().ReverseMap();
    }
}