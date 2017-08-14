using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Interfaces.Services
{
    public interface IMarcaService : IServiceBase<Marca>
    {
        Task InverteActiveStatus(string id);

        Task<IEnumerable<Marca>> Ativas();

        Task<IEnumerable<Marca>> PorCategoria(string category);

        Task<Marca> PorNome(string categoria, string nome);
    }
}
