namespace NetCoreTutorial.Repository
{
    public class GenericEntityRepository<T> : EntityRepository<T, MainContext> where T : class
    {
        public GenericEntityRepository(MainContext context) : base(context)
        {
        }
    }
}