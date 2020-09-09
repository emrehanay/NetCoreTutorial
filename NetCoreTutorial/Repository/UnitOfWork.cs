using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreTutorial.Helpers;

namespace NetCoreTutorial.Repository
{
    public interface IUnitOfWork<T> where T : IUnitOfWorkRepository
    {
        T Repository { get; }
        Task SaveChanges();
    }

    public class UnitOfWork<T> : IUnitOfWork<T> where T : IUnitOfWorkRepository
    {
        private readonly DbContext _context;
        public T Repository { get; }

        public UnitOfWork(T repository)
        {
            Repository = repository;
            _context = repository.GetDbContext();
        }

        public async Task SaveChanges()
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //TODO:Logging
            }
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}