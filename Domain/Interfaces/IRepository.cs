using System.Linq.Expressions;
using Domain.Seed;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces;

public interface IRepository<T, in TK, out TC>
    where T : Entity<TK>
    where TC : DbContext
{
    public TC Context { get; }

    public void Update(T obj);
    public Task<T> AddAsync(T obj);
    public void AddRange(params IEnumerable<T> obj);
    public void Delete(T obj);
    public Task SaveChangesAsync();
    public void SaveChanges();

    public Task<T?> FindAsync(TK id, params Expression<Func<T, object>>[] includes);
    public Task<T?> FindWhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    public Task<IEnumerable<T>> FindAllWhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
}