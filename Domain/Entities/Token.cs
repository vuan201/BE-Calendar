using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Extension;

namespace Domain.Entities;

[Table("Tokens")]
public class Token
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; } = string.Empty;
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }
    [Required]
    public string TokenValue { get; set; } = string.Empty;
    public long ExpiresTime { get; set; } = DateTime.UtcNow.ToUnixTime();
    public bool IsRevoked { get; set; } = false;
}
