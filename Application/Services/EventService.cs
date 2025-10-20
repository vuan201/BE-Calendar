using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, IMapper mapper)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
    }
    public async Task<List<EventDTO>> GetEventsAsync()
    {
        var result = await _eventRepository.GetListAsync();

        return _mapper.Map<List<EventDTO>>(result);
    }
}