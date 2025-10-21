
using Application.DTOs.EventDTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Event, EventDTO>().ReverseMap();
        CreateMap<Event, CreateEventDTO>().ReverseMap();
    }
}