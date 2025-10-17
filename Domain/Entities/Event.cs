using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums.Event;

namespace Domain.Entities;

[Table("Events")]
public class Event : EntityBase
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public string Title { get; set; } = string.Empty;

    [StringLength(640)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Priolity Priolity { get; set; } = Priolity.Default;

    [Required]
    public EventType EventType { get; set; } = EventType.Meeting;
    
    [Required]
    public RecurrenceRule RecurrenceRule { get; set; } = RecurrenceRule.NoRepeat;
    
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}