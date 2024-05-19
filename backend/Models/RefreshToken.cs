using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class RefreshToken
{
    [Key]
    public int TokenId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
}