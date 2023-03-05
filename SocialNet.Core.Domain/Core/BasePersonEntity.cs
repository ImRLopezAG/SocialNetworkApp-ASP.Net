namespace SocialNet.Core.Domain.Core;

public class BasePersonEntity : BaseEntity {
  public string Name { get; set; } = null!;
  public string LastName { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Phone { get; set; } = null!;
}
