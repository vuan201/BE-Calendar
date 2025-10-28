using System.ComponentModel.DataAnnotations;
using Domain.Enums.Event;

namespace Application.DTOs.EventDTO;

public class CreateEventDTO
{
    [Required, StringLength(128)]
    public string Title { get; set; } = string.Empty;
    [StringLength(640)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public Priolity Priolity { get; set; } = Priolity.Default;
    [Required]
    public EventType EventType { get; set; } = EventType.Meeting;
    [Required]
    public string RecurrenceRule { get; set; } = string.Empty;
    [Required]
    public long StartDate { get; set; }
    [Required]
    public long EndDate { get; set; }
}
