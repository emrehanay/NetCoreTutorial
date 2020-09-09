using NetCoreTutorial.Domain;

namespace NetCoreTutorial.Repository
{
    public class GenericBaseEntityRepository<T> : BaseEntityRepository<T, MainContext> where T : class, IBaseEntity
    {
        public GenericBaseEntityRepository(MainContext context) : base(context)
        {
        }
    }
}