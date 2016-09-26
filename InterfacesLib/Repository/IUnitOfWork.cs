using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace InterfacesLib.Repository
{
    public interface IUnitOfWork
    {
        DbSet<T> Set<T>() where T : class;
        //DbContext Ctx();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;


        int SaveChanges();
        System.Threading.Tasks.Task<int> SaveChangesAsync();
       

        void Dispose();
    }
}
