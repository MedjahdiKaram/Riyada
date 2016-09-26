using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using InterfacesLib.Repository;
using InterfacesLib.Shared;


namespace DAL
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<T> _entities;

        private  DbEntityEntry<T> _entry;
        public Repository(IUnitOfWork uow)
        {
            
            _uow = uow;
            _entities = _uow.Set<T>();
            
            //_dbContext = _uow.Ctx;
        }

        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public IQueryable<T> Get()
        {
            //var result = DbSet.ToList().AsQueryable();
            //return result;
            return _entities;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public T Get(int id)
        {
            return _entities.Find(id);
        }

        public virtual void Add(T entity)
        {
            //DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            
            //if (dbEntityEntry.State != EntityState.Detached)
            //{
            //    dbEntityEntry.State = EntityState.Added;
            //}
            //else
            _entry = _uow.Entry(entity);
            if (_entry.State != EntityState.Detached)
            {
                
                _entry.State = EntityState.Added;
            }



          

                //_entry.State = EntityState.Detached;
                _entities.Add(entity);
                _uow.SaveChanges();
               
                
                //DbSet.Add(entity);
                //DbContext.SaveChanges();
            //}
        }
        public virtual async void AddAsync(T entity)
        {

            _entry = _uow.Entry(entity);
            if (_entry.State != EntityState.Detached)
            {

                _entry.State = EntityState.Added;
            }

            _entities.Add(entity);
            await _uow.SaveChangesAsync();

        }

        public void AddOrUpdate(T entity)
        {
            _entities.AddOrUpdate(t => t.Id, entity);

            _uow.SaveChanges();
        }
        public void Update()
        {
            //DbSet.AddOrUpdate(entity);
            //DbContext.SaveChanges();
            _uow.SaveChanges();
        }

        public void Delete(T entity)
        {
            //DbSet.Remove(entity);
            //DbContext.SaveChanges();
            if (entity == null)
                throw new ArgumentNullException("Null result - Can not remove/delete a null Entity of " + typeof(T));
            _entities.Remove(entity);
            _uow.SaveChanges();
        }
   
        public void Delete(int id)
        {
            var entity = _entities.Find(id);
            if (entity == null)
                throw new ArgumentNullException("Null result - No "+typeof(T)+ " was found using the Id:"+id);
            _entities.Remove(entity);
            _uow.SaveChanges();
            //DbSet.Remove(DbSet.Find(id));
            //DbContext.SaveChanges();
        }

        public void Dispose()
        {
            //DbContext.Dispose();
            _uow.Dispose();
        }
    }
}
