using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Seed;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public abstract class Repository<T, TK, TC>(TC context) : IRepository<T, TK, TC>
    where T : Entity<TK>
    where TC : DbContext
{
    public TC Context => context;

    public void Update(T obj)
    {
        context.Update(obj);
    }

    public async Task<T> AddAsync(T obj)
    {
        var result = await context.AddAsync(obj);

        return result.Entity;
    }

    public void AddRange(IEnumerable<T> obj)
    {
        context.AddRange(obj);
    }

    public void Delete(T obj)
    {
        context.Remove(obj);
    }

    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public void SaveChanges()
    {
        context.SaveChanges();
    }
    
    public async Task<T?> FindAsync(TK id, params Expression<Func<T, object>>[] includes)
    {
        var query = context.Set<T>().AsQueryable();

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(p => p.Id != null && p.Id.Equals(id));
    }

    public async Task<T?> FindWhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = context.Set<T>().AsQueryable();

        if (includes.Length > 0)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> FindAllWhereAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = context.Set<T>().AsQueryable();

        if (includes.Length > 0)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.Where(predicate).ToListAsync();
    }
}