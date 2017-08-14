using System.Threading.Tasks;
using System.Collections.Generic;

namespace VeiculosWebApi.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task AddOrUpdateAsync(TEntity entity);

        Task<TEntity> FindAsync(string id);

        Task<IEnumerable<TEntity>> ListAsync(int size);

        Task<IEnumerable<TEntity>> ListAllAsync();

        Task DeleteAsync(string id);

        void Dispose();
    }
}
