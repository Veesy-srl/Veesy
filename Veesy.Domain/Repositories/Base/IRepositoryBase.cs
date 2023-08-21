using System.Linq.Expressions;

namespace Veesy.Domain.Repositories.Base;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    Task<T> Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}