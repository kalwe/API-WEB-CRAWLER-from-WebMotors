using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.DbContext;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Models;
using VeiculosWebApi.Repositories;

namespace VeiculosWebApi.Services
{
    public class ModeloService : ServiceBase<Modelo>, IModeloService
    {
        private static readonly IVeiculosDbContext db = new VeiculosDbContext();

        private static readonly IRepositoryBase<Marca> repositoryMarca = new RepositoryBase<Marca>(db);
        private readonly IMarcaService marcaService = new MarcaService(repositoryMarca);

        Modelo Modelo;

        public ModeloService(IRepositoryBase<Modelo> repository)
            : base(repository)
        {
            Modelo = new Modelo();
        }

        public async Task InverteActiveStatus(string id)
        {
            await SwitchInactiveStatus("modelos/"+id);
        }

        public async Task<IEnumerable<Modelo>> Ativos()
        {
            return Modelo.Ativos(await ListAllAsync());
        }

        public async Task<IEnumerable<Modelo>> PorMarca(string categoria, string marca)
        {
            return Modelo.PorMarca(await ListAllAsync(), "categorias/"+categoria, "marcas/"+marca);
        }
    }
}