using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Domain.Entities;
using SocialNet.Infrastructure.Persistence.Context;
using SocialNet.Infrastructure.Persistence.Core;

namespace SocialNet.Infrastructure.Persistence.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository {
  private readonly SocialNetContext _context;
  public PostRepository(SocialNetContext context) : base(context) => _context = context;
}
