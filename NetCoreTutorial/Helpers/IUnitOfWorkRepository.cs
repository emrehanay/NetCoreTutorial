using Microsoft.EntityFrameworkCore;

namespace NetCoreTutorial.Helpers
{
    public interface IUnitOfWorkRepository
    {
        DbContext GetDbContext();
    }
}