using System.Reflection.Metadata;
using Domain.Enums.Event;

namespace Application.Models.Queries;

public class EventFormQuery
{
    public long? FormDate { get; set; }
    public long? ToDate { get; set; }
    public string? Title { get; set; }
    public EventType? EventType { get; set; }
    public Priolity? Priolity { get; set; }
    public bool IsRecurrenceRule { get; set; } = true;
}
