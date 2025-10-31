using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums.Event;

namespace Domain.Entities;

[Table("Events")]
// * [Index(nameof(Id))]
public class Event : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }
    [Required]
    [StringLength(128)]
    public string Title { get; set; } = string.Empty;
    [StringLength(640)]
    public string? Description { get; set; }
    [Required]
    public Priolity Priolity { get; set; } = Priolity.Default;
    [Required]
    public EventType EventType { get; set; } = EventType.Meeting;
    [Required]
    public string? RecurrenceRule { get; set; }
    [Required]
    public long StartDate { get; set; }
    [Required]
    public long EndDate { get; set; }
}