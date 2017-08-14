using Raven.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using System;
using System.Linq;

namespace VeiculosWebApi.Repositories
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        private IVeiculosDbContext _db;

        public RepositoryBase(IVeiculosDbContext db)
        {
            _db = db;
        }

        public async Task AddOrUpdateAsync(TEntity entity)
        {
            using (IAsyncDocumentSession session = _db.OpenSessionAsync())
            {
                try
                {
                   await session.StoreAsync(entity);
                   await session.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                   Console.WriteLine(ex);
                }
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (IAsyncDocumentSession session = _db.OpenSessionAsync())
            {
                try
                {
                    session.Delete(id);
                    await session.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public async Task<TEntity> FindAsync(string id)
        {
            using (IAsyncDocumentSession session = _db.OpenSessionAsync())
            {
                try
                {
                    return await session.LoadAsync<TEntity>(id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }


        // Recebe o tamanho do retorno do database
        public async Task<IEnumerable<TEntity>> ListAsync(int size)
        {
            using (IAsyncDocumentSession session = _db.OpenSessionAsync())
            {
                try
                {
                    return await session.Query<TEntity>().Take(size).ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public async Task<IEnumerable<TEntity>> ListAllAsync()
        {
            using (IAsyncDocumentSession session = _db.OpenSessionAsync())
            {
                try
                {
                    var docs = new List<TEntity>();
                    var nextGroupOfDocs = new List<TEntity>();
                    const int TakeLimit = 512;
                    int i = 0;
                    int skipResults = 0;

                    do
                    {
                        nextGroupOfDocs = (List<TEntity>) await session.Query<TEntity>().Statistics(out RavenQueryStatistics stats).Skip(i * TakeLimit + skipResults).Take(TakeLimit).ToListAsync();
                        i++;
                        skipResults += stats.SkippedResults;

                        docs = docs.Concat(nextGroupOfDocs).ToList();
                    }
                    while (nextGroupOfDocs.Count == TakeLimit);

                    return docs;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }
        }

        public void Dispose()
        {
            Dispose();
        }
    }
}
