using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VeiculosWebApi.DbContext;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Models;
using VeiculosWebApi.Repositories;
using VeiculosWebApi.Services;

namespace VeiculosWebApi.Controllers
{
    [Route("[Controller]")]
    public class ModeloController : Controller
    {
        private static readonly IVeiculosDbContext db = new VeiculosDbContext();
        private static readonly IRepositoryBase<Modelo> repository = new RepositoryBase<Modelo>(db);
        private readonly IModeloService modeloService = new ModeloService(repository);

        //  POR MARCA E CATEGORIA
        [HttpGet("pormarca/{categoria}/{marca}")]
        public async Task<JsonResult> PorMarca(string categoria, string marca)
        {
            var result = await modeloService.PorMarca(categoria, marca);
            if (result.Count() != 0)
                return Json(result.ToList());
            else
                return Json(null);
        }

        // ATIVOS
        [HttpGet("ativos")]
        public async Task<JsonResult> Ativos()
        {
            var result = await modeloService.Ativos();
            if (result != null)
                return Json(result.ToList());
            else
                return Json(null);
        }

        // ADD OR UPDATE
        [HttpPost("addupdate/")]
        public async Task AddUpdateAsync(Modelo modelo)
        {
            modeloService.Add(modelo);
            await modeloService.CommitAsync();
        }

        // GET: {id}
        [HttpGet("{id}")]
        public async Task<JsonResult> Find(string id)
        {
            var result = await modeloService.FindAsync("modelos/"+id);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }

        // LIST:
        [HttpGet]
        public async Task<JsonResult> List()
        {
            var result = await modeloService.ListAllAsync();
            if (result != null)
                return Json(result.ToList());
            else
                return Json(null);
        }

        // POST: DELETE
        [HttpPost("delete/{id}")]
        public async Task Delete(string id)
        {
            await modeloService.SetInactiveStatus(id);
        }

        // POST: CONFIRMDELETE
        [HttpPost("confirmdelete/{id}")]
        public async Task ConfirmDelete(string id)
        {
            await modeloService.DeleteAsync("modelos/"+id);
        }
    }
}