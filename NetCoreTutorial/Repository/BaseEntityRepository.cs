using Microsoft.EntityFrameworkCore;
using NetCoreTutorial.Domain;

namespace NetCoreTutorial.Repository
{
    public abstract class BaseEntityRepository<TEntity, TContext> : EntityRepository<TEntity, TContext>
        where TEntity : class, IBaseEntity
        where TContext : DbContext
    {
        protected BaseEntityRepository(TContext context) : base(context)
        {
        }

        public new void Remove(TEntity entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }
    }
}