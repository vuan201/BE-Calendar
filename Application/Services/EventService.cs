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

    public async Task<ViewModel<EventDTO>> CreateEventAsync(CreateEventDTO eventDto, string? userId)
    {
        try
        {
            if (string.IsNullOrEmpty(eventDto.RecurrenceRule))
            {
                var newEvent = _mapper.Map<Event>(eventDto);
                newEvent.UserId = userId;
                await _eventRepository.CreateAsync(newEvent);
                await _eventRepository.SaveChangesAsync();

                return new ViewModel<EventDTO>(true, "Event created successfully.", _mapper.Map<EventDTO>(newEvent), 1);
            }

            using (var reader = new StringReader(eventDto.RecurrenceRule))
            {
                // * Thử tải xem đó có phải là chuỗi lịch đầy đủ hay chỉ là quy tắc.
                var icalString = eventDto.RecurrenceRule.Contains("BEGIN:VCALENDAR") 
                    ? eventDto.RecurrenceRule 
                    : $"BEGIN:VCALENDAR\r\nBEGIN:VEVENT\r\nRRULE:{eventDto.RecurrenceRule}\r\nEND:VEVENT\r\nEND:VCALENDAR";
                
                using (var icalReader = new StringReader(icalString))
                {
                    var calendar = Calendar.Load(icalReader);
                    if (calendar != null)
                    {
                        var newEvent = _mapper.Map<Event>(eventDto);
                        newEvent.UserId = userId;
                        await _eventRepository.CreateAsync(newEvent);
                        await _eventRepository.SaveChangesAsync();

                        return new ViewModel<EventDTO>(true, "Event created successfully.", _mapper.Map<EventDTO>(newEvent), 1);
                    }
                    throw new Exception("Invalid recurrence rule format.");
                }
            }
        }
        catch (Exception ex)
        {
            return new ViewModel<EventDTO>(false, ex.Message);
        }
    }

    public async Task<ViewModel<EventDTO>> UpdateEventAsync(int id, CreateEventDTO eventDto, string? userId)
    {
        try
        {
            var oldEvent = await _eventRepository.GetAsync(i => i.Id == id && i.UserId == userId);
            if (oldEvent == null)
            {
                return new ViewModel<EventDTO>(false, "Event not found or access denied.");
            }

            _mapper.Map(eventDto, oldEvent);
            _eventRepository.Update(oldEvent);
            await _eventRepository.SaveChangesAsync();

            return new ViewModel<EventDTO>(true, "Event updated successfully.", _mapper.Map<EventDTO>(oldEvent), 1);
        }
        catch (Exception ex)
        {
            return new ViewModel<EventDTO>(false, ex.Message);
        }
    }

    public async Task<ViewModel> DeleteEventAsync(int id, string? userId)
    {
        var oldEvent = await _eventRepository.GetAsync(i => i.Id == id && (string.IsNullOrEmpty(userId) || i.UserId == userId));

        if (oldEvent != null)
        {
            _eventRepository.Delete(oldEvent);
            await _eventRepository.SaveChangesAsync();
            return new ViewModel(true, "Delete event success");
        }

        return new ViewModel(false, "Delete event failed or access denied");
    }

    public async Task<ViewModel<EventDTO>> GetEventByIdAsync(int id, string? userId)
    {
        var result = await _eventRepository.GetAsync(i => i.Id == id && (string.IsNullOrEmpty(userId) || i.UserId == userId));
        if (result is not null)
        {
            return new ViewModel<EventDTO>(true, "Get data success", _mapper.Map<EventDTO>(result), 1);
        }
        return new ViewModel<EventDTO>(false, "Get data failed");
    }

    public async Task<ViewModel<List<EventDTO>>> GetEventsAsync(EventFormQuery query, string? userId)
    {
        // * Lọc dữ liệu theo các điều kiện được truyền vào
        var filter = PredicateBuilder.New<Event>(true);

        if (!string.IsNullOrEmpty(userId))
        {
            filter.And(e => e.UserId == userId);
        }

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