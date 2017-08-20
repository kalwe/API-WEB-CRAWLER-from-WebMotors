using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Services
{
    public class ModeloService : ServiceBase<Modelo>, IModeloService
    {
        Modelo Modelo;

        public ModeloService(IModeloRepository repository,
                    ISwitchActiveStatusService<Modelo> switchActiveStatus)
            : base(repository, switchActiveStatus)
        {
            Modelo = new Modelo();
        }

        public async Task SetInactiveStatus(string id)
        {
            await AddUpdateAsync(SetActiveStatusFalse(await FindAsync("modelos/"+id)));
        }

        public async Task SetActiveStatus(string id)
        {
            await AddUpdateAsync(SetActiveStatusTrue(await FindAsync("modelos/"+id)));
        }

        public async Task<IEnumerable<Modelo>> Ativos()
        {
            return Modelo.ActiveTrue(await ListAllAsync());
        }

        public async Task<IEnumerable<Modelo>> PorMarca(string categoria, string marca)
        {
            return Modelo.PorMarca(await ListAllAsync(), "categorias/"+categoria, "marcas/"+marca);
        }
    }
}