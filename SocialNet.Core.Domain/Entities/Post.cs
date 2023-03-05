using SocialNet.Core.Domain.Core;

namespace SocialNet.Core.Domain.Entities;

public class Post : BaseEntity {
  public string? Content { get; set; }
  public string? Image { get; set; }
  public int UserId { get; set; }
  public User User { get; set; } = null!;
  public ICollection<Comment>? Comments { get; set; }
}
