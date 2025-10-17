using Domain.Entities;

namespace Application.Interfaces;

public interface IEventService
{
    Task<List<Event>> GetEventsAsync();
}