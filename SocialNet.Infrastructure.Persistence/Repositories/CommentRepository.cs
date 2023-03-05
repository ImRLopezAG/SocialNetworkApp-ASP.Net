using SocialNet.Core.Application.Interfaces;
using SocialNet.Core.Domain.Entities;
using SocialNet.Infrastructure.Persistence.Context;
using SocialNet.Infrastructure.Persistence.Core;

namespace SocialNet.Infrastructure.Persistence.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository {
  private readonly SocialNetContext _context;
  public CommentRepository(SocialNetContext context) : base(context) => _context = context;

}
