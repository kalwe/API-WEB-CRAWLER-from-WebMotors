using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Controllers
{
    [Route("[Controller]")]
    public class ModeloController : Controller
    {
        private readonly IModeloService _modeloService;

        public ModeloController(IModeloService modeloService)
        {
            _modeloService = modeloService;
        }

        //  POR MARCA E CATEGORIA
        [HttpGet("pormarca/{categoria}/{marca}")]
        public async Task<JsonResult> PorMarca(string categoria, string marca)
        {
            var result = await _modeloService.PorMarca(categoria, marca);
            if (result.Count() != 0)
                return Json(result.ToList());
            else
                return Json(null);
        }

        // ATIVOS
        [HttpGet("ativos")]
        public async Task<JsonResult> Ativos()
        {
            var result = await _modeloService.Ativos();
            if (result != null)
                return Json(result.ToList());
            else
                return Json(null);
        }

        // ADD OR UPDATE
        [HttpPost("addupdate/")]
        public async Task AddUpdateAsync(Modelo modelo)
        {
            _modeloService.Add(modelo);
            await _modeloService.CommitAsync();
        }

        // GET: {id}
        [HttpGet("{id}")]
        public async Task<JsonResult> Find(string id)
        {
            var result = await _modeloService.FindAsync("modelos/"+id);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }

        // LIST:
        [HttpGet]
        public async Task<JsonResult> List()
        {
            var result = await _modeloService.ListAllAsync();
            if (result != null)
                return Json(result.ToList());
            else
                return Json(null);
        }

        // POST: DELETE
        [HttpGet("delete/{id}")]
        public async Task Delete(string id)
        {
            await _modeloService.SetInactiveStatus(id);
        }

        // POST: CONFIRMDELETE
        [HttpPost("confirmdelete/{id}")]
        public async Task ConfirmDelete(string id)
        {
            await _modeloService.DeleteAsync("modelos/"+id);
        }
    }
}