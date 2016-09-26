using System.Data.Entity;
using InterfacesLib.Repository;
using ModelsLib;

namespace DAL
{
    public class GlobalContext:DbContext,IUnitOfWork
    {
        public DbSet<Login> Logins { get; set; }
        public DbSet<Client> Clients                    { get; set; }
        public DbSet<History> Histories                 { get; set; }
        public DbSet<Subcription> Subcriptions          { get; set; }
        public DbSet<SubcriptionType> SubcriptionTypes  { get; set; }
        public DbSet<Payement> Payements                { get; set; }

        public GlobalContext()
            : base("name=ConnectionSqlServerCe")
        {
            Database.CreateIfNotExists();
            
        }
        
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Examen>().HasOptional(p => p.Medecin)
        //        .WithMany(b => b.Examen)
        //        .HasForeignKey(p => p.ExamenId);
        //}


    
    }
}
