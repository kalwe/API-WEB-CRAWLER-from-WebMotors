using System.Collections.Generic;
using System.Threading.Tasks;

namespace VeiculosWebApi.Interfaces.Services
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        Task CommitAsync();

        Task AddUpdateAsync(TEntity entity);

        Task<TEntity> FindAsync(string id);

        Task<IEnumerable<TEntity>> ListAsync(int size);

        Task<IEnumerable<TEntity>> ListAllAsync();

        Task DeleteAsync(string id);
    }
}
