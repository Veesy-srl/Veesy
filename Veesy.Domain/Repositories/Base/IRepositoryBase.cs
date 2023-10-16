using System.Linq.Expressions;

namespace Veesy.Domain.Repositories.Base;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    Task<T> Create(T entity);
    T Update(T entity);
    List<T> UpdateRange(List<T> entities);
    T Delete(T entity);
    void DeleteRange(List<T> entities);
}