using System;
using System.Linq;
using System.Linq.Expressions;

namespace InterfacesLib.Repository
{
    public interface IRepository<T> where T : class 
    {
        IQueryable<T> Get();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        T Get(int id);
        void Add(T entity);
        void AddAsync(T entity);
        void AddOrUpdate(T entity);
        void Update();
        void Delete(T entity);
        void Delete(int id);
    }
}
