using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreTutorial.Domain;

namespace NetCoreTutorial.Repository
{
    public class MainContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasQueryFilter(m => EF.Property<bool>(m, "IsDeleted") == false);
        }

        public override int SaveChanges()
        {
            SaveTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SaveTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SaveTimestamps()
        {
            var now = DateTime.Now;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is IBaseEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedAt = now;
                            entity.UpdatedAt = now;
                            break;

                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                            entity.UpdatedAt = now;
                            break;
                    }
                }
            }
        }
    }
}