using Application.DTOs;

namespace Application.Interfaces;

public interface IEventService
{
    Task<List<EventDTO>> GetEventsAsync();
}