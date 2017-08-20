using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Services
{
    public class MarcaService : ServiceBase<Marca>, IMarcaService
    {
        private Marca Marca;

        public MarcaService(IMarcaRepository repository,
                    ISwitchActiveStatusService<Marca> switchActiveStatus)
            : base(repository, switchActiveStatus)
        {
            Marca = new Marca();
        }

        public async Task SetInactiveStatus(string id)
        {
            await AddUpdateAsync(SetActiveStatusFalse(await FindAsync("marcas/"+id)));
        }

        public async Task SetActiveStatus(string id)
        {
            await AddUpdateAsync(SetActiveStatusTrue(await FindAsync("marcas/"+id)));
        }

        // Retorna todas as marcas com status ativo
        public async Task<IEnumerable<Marca>> Ativas()
        {
            return Marca.ActiveTrue(await ListAllAsync());
        }

        // Retorna todas as marcas de uma categoria
        public async Task<IEnumerable<Marca>> PorCategoria(string categoria)
        {
            return Marca.PorCategoria(await ListAllAsync(), "categorias/"+categoria.ToLower());
        }

        // Retorna todas as marcas de uma categoria por nome
        public async Task<Marca> PorCategoriaENome(string categoria, string nome)
        {
            return Marca.PorCategoriaENome(await ListAllAsync(), "categorias/"+categoria.ToLower(), nome.ToUpper());
        }
    }
}