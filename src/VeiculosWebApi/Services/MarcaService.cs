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

        // Default Constructor
        public MarcaService(IRepositoryBase<Marca> repository) 
            : base(repository)
        {
            Marca = new Marca();
        }

        // Switch active status
        public async Task InverteActiveStatus(string id)
        {
            await SwitchInactiveStatus("marcas/"+id);
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