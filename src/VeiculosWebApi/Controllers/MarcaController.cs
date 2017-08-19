using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using VeiculosWebApi.Models;
using VeiculosWebApi.Interfaces.Services;

namespace VeiculosWebApi.Controllers
{
    [Route("[Controller]")]
    public class MarcaController : Controller
    {
        private readonly IMarcaService _marcaService;

        public MarcaController(IMarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        // POST: Add Or Update
        [HttpPost("addupdate/")]
        public async Task AddUpdateAsync(Marca marca)
        {
            marca.Name = marca.Name.ToUpper();
            _marcaService.Add(marca);
            await _marcaService.CommitAsync();
        }

        // GET
        [HttpGet("{id}")]
        public async Task<JsonResult> Find(string id)
        {
            var result = await _marcaService.FindAsync("marcas/"+id);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }

        // GET: marcas
        public async Task<JsonResult> List()
        {
            return Json(await _marcaService.ListAllAsync());
        }

        // DELETE
        [HttpPost("delete/{id}")]
        public async Task Delete(string id)
        {
            await _marcaService.SetInactiveStatus(id);
        }

        [HttpPost("confirmdelete/{id}")]
        public async Task ConfirmDelete(string id)
        {
            await _marcaService.DeleteAsync("marcas/"+id);
        }

        // GET: ativas
        [HttpGet("ativas/")]
        public async Task<JsonResult> Active()
        {
            var result = await _marcaService.Ativas();
            if (result.Count() != 0)
                return Json(result.ToList());
            else 
                return Json(null);
        }

        // GET: MARCAS POR CATEGORIA
        [HttpGet("porcategoria/{categoria}")]
        public async Task<JsonResult> PorCategoria(string categoria)
        {
            var marcas = await _marcaService.PorCategoria(categoria);
            if (marcas.Count() != 0)
                return Json(marcas.ToList());
            else
                return Json(null);
        }

        // GET: MARCA POR CATEGORIA E NOME
        [HttpGet("porcategoriaenome/{categoria}/{nome}")]
        public async Task<JsonResult> PorCategoriaENome(string categoria, string nome)
        {
            var result = await _marcaService.PorCategoriaENome(categoria, nome);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }
    }
}
