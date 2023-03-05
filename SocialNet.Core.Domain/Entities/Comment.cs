using SocialNet.Core.Domain.Core;

namespace SocialNet.Core.Domain.Entities;

public class Comment : BaseEntity {
  public string Content { get; set; } = null!;
  public int UserId { get; set; }
  public User User { get; set; } = null!;
  public int PostId { get; set; }
  public Post Post { get; set; } = null!;
}

