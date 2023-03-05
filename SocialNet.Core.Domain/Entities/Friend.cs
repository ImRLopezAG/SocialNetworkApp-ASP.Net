using SocialNet.Core.Domain.Core;

namespace SocialNet.Core.Domain.Entities;

public class Friend : BaseEntity {
  public int SenderId { get; set; }

  public int ReceptorId { get; set; }


  public User Sender { get; set; } = null!;

}