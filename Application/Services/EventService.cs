using Application.DTOs.EventDTO;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Ical.Net;
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

    public async Task<ViewModel<EventDTO>> CreateEventAsync(CreateEventDTO eventDto)
    {
        try
        {
            using (var reader = new StreamReader(eventDto.RecurrenceRule))
            {
                var calendar = Calendar.Load(reader);
                if (calendar != null)
                {
                    var newEvent = _mapper.Map<Event>(eventDto);
                    await _eventRepository.CreateAsync(newEvent);
                    await _eventRepository.SaveChangesAsync();

                    return new ViewModel<EventDTO>
                    {
                        Data = _mapper.Map<EventDTO>(newEvent),
                        Message = "Event created successfully.",
                        Status = true,
                        Count = 1
                    };
                }
                throw new Exception("Invalid recurrence rule format.");
            }
        }
        catch (Exception ex)
        {
            return new ViewModel<EventDTO>
            {
                Message = ex.Message,
                Status = false,
            };
        }
    }

    public async Task<ViewModel> DeleteEventAsync(int id)
    {
        var oldEvent = await _eventRepository.GetAsync(i => i.Id == id);

        if (oldEvent != null)
        {
            _eventRepository.Delete(oldEvent);
            await _eventRepository.SaveChangesAsync();
            return new ViewModel { Status = true, Message = "Delete event success" };
        }

        return new ViewModel { Status = false, Message = "Delete event failed" };
    }

    public async Task<ViewModel<EventDTO>> GetEventByIdAsync(int id)
    {
        var result = await _eventRepository.GetAsync(i => i.Id == id);
        if (result is not null)
        {
            return new ViewModel<EventDTO>
            {
                Data = _mapper.Map<EventDTO>(result),
                Message = "Get data success",
                Status = true,
                Count = 1
            };
        }
        return new ViewModel<EventDTO> { Status = false, Message = "Data does not exist" };
    }

    public async Task<ViewModel<List<EventDTO>>> GetEventsAsync()
    {
        var result = await _eventRepository.GetListAsync();
        if (result is not null)
        {
            return new ViewModel<List<EventDTO>>
            {
                Data = _mapper.Map<List<EventDTO>>(result),
                Message = "Get data success",
                Status = true,
                Count = result.Count()
            };
        }
        return new ViewModel<List<EventDTO>> { Status = false, Message = "Data does not exist" };
    }
}