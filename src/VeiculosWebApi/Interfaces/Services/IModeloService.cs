using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Interfaces.Services
{
    public interface IModeloService : IServiceBase<Modelo>
    {
        Task SetInactiveStatus(string id);

        Task SetActiveStatus(string id);

        Task<IEnumerable<Modelo>> Ativos();

        Task<IEnumerable<Modelo>> PorMarca(string categoria, string marca);
    }
}