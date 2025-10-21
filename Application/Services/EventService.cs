using System.Linq.Expressions;
using Application.DTOs.EventDTO;
using Application.Interfaces;
using Application.Models;
using Application.Models.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Ical.Net;
using LinqKit;

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

    public async Task<ViewModel<List<EventDTO>>> GetEventsAsync(EventFormQuery query)
    {
        // Lọc dữ liệu theo các điều kiện được truyền vào
        var filter = PredicateBuilder.New<Event>(true);

        if (query.FormDate.HasValue || query.ToDate.HasValue)
        {
            // Chỉ lấy giờ UTC
            if (query.FormDate.HasValue && query.FormDate.Value.Kind != DateTimeKind.Utc)
            {
                filter.And(e => e.StartDate >= query.FormDate!.Value.ToUniversalTime());
            }
            else if(query.ToDate.HasValue && query.ToDate.Value.Kind != DateTimeKind.Utc)
            {
                filter.And(e => e.EndDate <= query.ToDate!.Value);
            }
            return new ViewModel<List<EventDTO>>
            {
                Message = "Please use UTC date and time for the form and to dates.",
                Status = false,
                Count = 0
            };
        }
        if (!string.IsNullOrEmpty(query.Title))
        {
            filter.And(e => e.Title.Contains(query.Title));
        }
        if (query.EventType.HasValue)
        {
            filter.And(e => e.EventType == query.EventType);
        }
        if (query.Priolity.HasValue)
        {
            filter.And(e => e.Priolity == query.Priolity);
        }
        if (!query.IsRecurrenceRule)
        {
            filter.And(e => string.IsNullOrEmpty(e.RecurrenceRule));
        }

        var result = await _eventRepository.GetListAsync(filter);
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