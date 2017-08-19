using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using VeiculosWebApi.Models;
using VeiculosWebApi.Interfaces.Services;

namespace VeiculosWebApi.Controllers
{
    [Route("[controller]")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        public async Task<JsonResult> PorNome(string nome)
        {
            var result = await _categoriaService.PorNome(nome);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }

        // POST: api/categoria/addupdate
        [HttpPost("addupdate/")]
        public async Task AddOrUpdate(Categoria categoria)
        {
            _categoriaService.Add(categoria);
            await _categoriaService.CommitAsync();
        }

        // GET: api/categoria/{string:id}
        [HttpGet("{id}")]
        public async Task<JsonResult> Find(string id)
        {
            var result = await _categoriaService.FindAsync("categorias/"+id);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }

        [HttpGet]
        public async Task<JsonResult> List()
        {
            var result = await _categoriaService.ListAsync(256);
            if (result != null)
                return Json(result.ToList());
            else
                return Json(null);
        }

        // DELETE /categoria/delete/{string:id}
        [HttpPost("delete/{id}")]
        public async Task Delete(string id)
        {
            await _categoriaService.SetInactiveStatus("categorias/"+id);
        }

        // DELETE CONFIRM
        [HttpPost("confirmdelete/{id}")]
        public async Task ConfirmDelete(string id)
        {
            await _categoriaService.DeleteAsync("categorias/"+id);
        }

        // GET api/categoria/active
        [HttpGet("ativas/")]
        public async Task<JsonResult> Active()
        {
            var result = await _categoriaService.Ativas();
            if (result.Count() != 0)
                return Json(result.ToList());
            else
                return Json(null);
        }
    } 
}