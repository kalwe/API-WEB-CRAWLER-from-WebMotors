using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Interfaces.Services
{
    public interface ICategoriaService : IServiceBase<Categoria>
    {
        Task SetInactiveStatus(string id);

        Task SetActiveStatus(string id);

        Task<IEnumerable<Categoria>> Ativas();

        Task<Categoria> PorNome(string nome);
    }
}