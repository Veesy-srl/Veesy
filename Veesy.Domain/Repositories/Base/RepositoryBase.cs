using System;
using System.Collections.Generic;
using System.Linq;
using Veesy.Domain.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Veesy.Domain.Repositories.Base;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected ApplicationDbContext _applicationDbContext { get; set; }
    public RepositoryBase(ApplicationDbContext applicationDbContext)
    {
        this._applicationDbContext = applicationDbContext;
    }
    public IQueryable<T> FindAll() 
    {
        return this._applicationDbContext.Set<T>().AsNoTracking();
    }
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return this._applicationDbContext.Set<T>()
            .Where(expression).AsNoTracking();
    }
    public async Task<T> Create(T entity)
    {
        await this._applicationDbContext.Set<T>().AddAsync(entity);
        return entity;
    }
    public T Update(T entity)
    {
        this._applicationDbContext.Set<T>().Update(entity);
        return entity;
    }

    public List<T> UpdateRange(List<T> entities)
    {
        _applicationDbContext.Set<T>().UpdateRange(entities);
        return entities;
    }

    public T Delete(T entity)
    {
        this._applicationDbContext.Set<T>().Remove(entity);
        return entity;
    }

    public void DeleteRange(List<T> entities)
    {
        this._applicationDbContext.Set<T>().RemoveRange(entities);
    }
}