using SocialNet.Core.Domain.Core;

namespace SocialNet.Core.Domain.Entities;

public class User : BasePersonEntity {
  public string UserName { get; set; } = null!;
  public string Password { get; set; } = null!;
  public string? Image { get; set; } = null!;
  public bool IsConfirmed { get; set; }

  public ICollection<Post> Posts { get; set; } = null!;
  public ICollection<Comment> Comments { get; set; } = null!;
  public ICollection<Friend> Friends { get; set; } = null!;
}
