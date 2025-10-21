using Application.DTOs.EventDTO;
using Application.Models;

namespace Application.Interfaces;

public interface IEventService
{
    Task<ViewModel<EventDTO>> CreateEventAsync(CreateEventDTO eventDto);
    Task<ViewModel> DeleteEventAsync(int id);
    Task<ViewModel<EventDTO>> GetEventByIdAsync(int id);
    Task<ViewModel<List<EventDTO>>> GetEventsAsync();
}