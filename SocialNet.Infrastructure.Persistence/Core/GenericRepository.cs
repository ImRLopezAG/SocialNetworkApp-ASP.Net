using Microsoft.EntityFrameworkCore;
using SocialNet.Core.Application.Core;
using SocialNet.Core.Domain.Core;
using SocialNet.Infrastructure.Persistence.Context;
using System.Linq.Expressions;

namespace SocialNet.Infrastructure.Persistence.Core;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity {
  private readonly SocialNetContext _context;

  public GenericRepository(SocialNetContext context) => _context = context;

  public virtual async Task<IEnumerable<TEntity>> GetAll() => await _context.Set<TEntity>().OrderByDescending(x => x.CreatedAt).ToListAsync();
  public virtual async Task<TEntity> GetEntity(int id) => await _context.Set<TEntity>().FindAsync(id);

  public async Task<bool> Exists(Expression<Func<TEntity, bool>> Filter) => await _context.Set<TEntity>().AnyAsync(Filter);

  public virtual async Task<TEntity> Save(TEntity entity) {
    await _context.Set<TEntity>().AddAsync(entity);
    await _context.SaveChangesAsync();
    return entity;
  }

  public virtual async Task Update(TEntity entity) {
    var entry = await _context.Set<TEntity>().FindAsync(entity.Id);
    _context.Entry(entry).CurrentValues.SetValues(entity);
    await _context.SaveChangesAsync();
  }

  public virtual async Task Delete(TEntity entity) {
    _context.Set<TEntity>().Remove(entity);
    await _context.SaveChangesAsync();
  }
}
