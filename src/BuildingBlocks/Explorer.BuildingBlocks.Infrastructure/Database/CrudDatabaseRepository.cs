using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Explorer.BuildingBlocks.Infrastructure.Database;

public class CrudDatabaseRepository<TEntity, TDbContext> : ICrudRepository<TEntity>
    where TEntity : Entity
    where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;
    private readonly DbSet<TEntity> _dbSet;

    public CrudDatabaseRepository(TDbContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TEntity>();
    }

    public PagedResult<TEntity> GetPaged(int page, int pageSize)
    {
        var task = _dbSet.GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public TEntity Get(long id, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        // Uključi sve navedene entitete
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        var entity = query.FirstOrDefault(e => e.Id == id);
        if (entity == null) throw new KeyNotFoundException("Not found: " + id);
        return entity;
    }
    public TEntity Get(long id)
    {
        var entity = _dbSet.Find(id);
        if (entity == null) throw new KeyNotFoundException("Not found: " + id);
        return entity;
    }

    public TEntity Create(TEntity entity)
    {
        var provjera = _dbSet.Add(entity);
        DbContext.SaveChanges();
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        try
        {
            DbContext.Update(entity);
            DbContext.SaveChanges();
        }
        catch (DbUpdateException e)
        {
            throw new KeyNotFoundException(e.Message);
        }
        return entity;
    }

    public void Delete(long id)
    {
        var entity = Get(id);
        
        _dbSet.Remove(entity);
        DbContext.SaveChanges();
    }
}