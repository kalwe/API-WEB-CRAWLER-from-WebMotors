using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Services
{
    public class CategoriaService : ServiceBase<Categoria>, ICategoriaService
    {
        private Categoria Categoria;

        public CategoriaService(ICategoriaRepository repository,
                    ISwitchActiveStatusService<Categoria> switchActiveStatus)
            : base(repository, switchActiveStatus)
        {
            Categoria = new Categoria();
        }

        public async Task SetInactiveStatus(string id)
        {
            await AddUpdateAsync(SetActiveStatusFalse(await FindAsync(id)));
        }

        public async Task<IEnumerable<Categoria>> Ativas()
        {
            return Categoria.ActiveTrue(await ListAllAsync());
        }

        public async Task<Categoria> PorNome(string nome)
        {
            return Categoria.PorNome(await ListAllAsync(), nome);
        }

        public async Task SetActiveStatus(string id)
        {
            await AddUpdateAsync(SetActiveStatusTrue(await FindAsync(id)));
        }
    }
}