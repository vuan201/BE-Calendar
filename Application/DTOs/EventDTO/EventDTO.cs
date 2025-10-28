using Domain.Enums.Event;

namespace Application.DTOs.EventDTO;

public class EventDTO
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priolity Priolity { get; set; } = Priolity.Default;
    public EventType EventType { get; set; } = EventType.Meeting;
    public string RecurrenceRule { get; set; } = string.Empty;
    public long StartDate { get; set; }
    public long EndDate { get; set; }
}