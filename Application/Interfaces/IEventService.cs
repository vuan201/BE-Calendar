using Application.DTOs.EventDTO;
using Application.Models;
using Application.Models.Queries;

namespace Application.Interfaces;

public interface IEventService
{
    Task<ViewModel<EventDTO>> CreateEventAsync(CreateEventDTO eventDto, string? userId);
    Task<ViewModel<EventDTO>> UpdateEventAsync(int id, CreateEventDTO eventDto, string? userId);
    Task<ViewModel> DeleteEventAsync(int id, string? userId);
    Task<ViewModel<EventDTO>> GetEventByIdAsync(int id, string? userId);
    Task<ViewModel<List<EventDTO>>> GetEventsAsync(EventFormQuery query, string? userId);
}