using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using VeiculosWebApi.DbContext;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Models;
using VeiculosWebApi.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Services;

namespace VeiculosWebApi.Controllers
{
    [Route("[Controller]")]
    public class MarcaController : Controller
    {
        private static readonly IVeiculosDbContext db = new VeiculosDbContext();
        private static readonly IRepositoryBase<Marca> repositoryBase = new RepositoryBase<Marca>(db);
        private readonly IMarcaService marcaService = new MarcaService(repositoryBase);

        // POST: Add Or Update
        [HttpPost("addupdate/")]
        public async Task AddUpdateAsync(Marca marca)
        {
            marca.Name = marca.Name.ToUpper();
            marcaService.Add(marca);
            await marcaService.CommitAsync();
        }

        // GET
        [HttpGet("{id}")]
        public async Task<JsonResult> Find(string id)
        {
            var result = await marcaService.FindAsync("marcas/"+id);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }

        // GET: marcas
        public async Task<JsonResult> List()
        {
            return Json(await marcaService.ListAllAsync());
        }

        // DELETE
        [HttpPost("delete/{id}")]
        public async Task Delete(string id)
        {
            await marcaService.InverteActiveStatus(id);
        }

        [HttpPost("confirmdelete/{id}")]
        public async Task ConfirmDelete(string id)
        {
            await marcaService.DeleteAsync("marcas/"+id);
        }

        // GET: ativas
        [HttpGet("ativas/")]
        public async Task<JsonResult> Active()
        {
            var result = await marcaService.Ativas();
            if (result.Count() != 0)
                return Json(result.ToList());
            else 
                return Json(null);
        }

        // GET: MARCAS POR CATEGORIA
        [HttpGet("porcategoria/{categoria}")]
        public async Task<JsonResult> PorCategoria(string categoria)
        {
            var marcas = await marcaService.PorCategoria(categoria);
            if (marcas.Count() != 0)
                return Json(marcas.ToList());
            else
                return Json(null);
        }

        // GET: MARCA POR NOME E CATEGORIA
        [HttpGet("pornome/{categoria}/{nome}")]
        public async Task<JsonResult> PorNome(string categoria, string nome)
        {
            var result = await marcaService.PorNome(categoria, nome);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }
    }
}
