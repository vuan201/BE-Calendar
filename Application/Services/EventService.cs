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

                    return new ViewModel<EventDTO>(true, "Event created successfully.", _mapper.Map<EventDTO>(newEvent), 1);
                }
                throw new Exception("Invalid recurrence rule format.");
            }
        }
        catch (Exception ex)
        {
            return new ViewModel<EventDTO>(false, ex.Message);
        }
    }

    public async Task<ViewModel> DeleteEventAsync(int id)
    {
        var oldEvent = await _eventRepository.GetAsync(i => i.Id == id);

        if (oldEvent != null)
        {
            _eventRepository.Delete(oldEvent);
            await _eventRepository.SaveChangesAsync();
            return new ViewModel(true, "Delete event success");
        }

        return new ViewModel(false, "Delete event failed");
    }

    public async Task<ViewModel<EventDTO>> GetEventByIdAsync(int id)
    {
        var result = await _eventRepository.GetAsync(i => i.Id == id);
        if (result is not null)
        {
            return new ViewModel<EventDTO>(true, "Get data success", _mapper.Map<EventDTO>(result), 1);
        }
        return new ViewModel<EventDTO>(false, "Get data failed");
    }

    public async Task<ViewModel<List<EventDTO>>> GetEventsAsync(EventFormQuery query)
    {
        // * Lọc dữ liệu theo các điều kiện được truyền vào
        var filter = PredicateBuilder.New<Event>(true);

        if (query.FormDate.HasValue || query.ToDate.HasValue)
        {
            // * Chỉ lấy giờ UTC
            if (query.FormDate != null)
            {
                filter.And(e => e.StartDate >= query.FormDate);
            }
            if (query.ToDate != null)
            {
                filter.And(e => e.EndDate <= query.ToDate!.Value);
            }
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
            return new ViewModel<List<EventDTO>>(true, "Get data success", _mapper.Map<List<EventDTO>>(result), result.Count());
        }
        return new ViewModel<List<EventDTO>>(false, "Data does not exist");
    }
}