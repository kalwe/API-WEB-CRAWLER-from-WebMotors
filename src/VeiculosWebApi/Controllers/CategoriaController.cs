using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using VeiculosWebApi.DbContext;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Models;
using VeiculosWebApi.Services;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Repositories;
using VeiculosWebApi.Interfaces.Repositories;
using System;

namespace VeiculosWebApi.Controllers
{
    [Route("[controller]")]
    public class CategoriaController : Controller
    {
        private static readonly IVeiculosDbContext db = new VeiculosDbContext();
        private static readonly IRepositoryBase<Categoria> repositoryBase = new RepositoryBase<Categoria>(db);
        private readonly ICategoriaService categoriaService = new CategoriaService(repositoryBase);

        public async Task<JsonResult> PorNome(string nome)
        {
            var result = await categoriaService.PorNome(nome);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }

        // POST: api/categoria/addupdate
        [HttpPost("addupdate/")]
        public async Task AddOrUpdate(Categoria categoria)
        {
            categoriaService.Add(categoria);
            await categoriaService.CommitAsync();
        }

        // GET: api/categoria/{string:id}
        [HttpGet("{id}")]
        public async Task<JsonResult> Find(string id)
        {
            var result = await categoriaService.FindAsync("categorias/"+id);
            if (result != null)
                return Json(result);
            else
                return Json(null);
        }

        [HttpGet]
        public async Task<JsonResult> List()
        {
            var result = await categoriaService.ListAsync(256);
            if (result != null)
                return Json(result.ToList());
            else
                return Json(null);
        }

        // DELETE /categoria/delete/{string:id}
        [HttpPost("delete/{id}")]
        public async Task Delete(string id)
        {
            await categoriaService.SetInactiveStatus("categorias/"+id);
        }

        // DELETE CONFIRM
        [HttpPost("confirmdelete/{id}")]
        public async Task ConfirmDelete(string id)
        {
            await categoriaService.DeleteAsync("categorias/"+id);
        }

        // GET api/categoria/active
        [HttpGet("ativas/")]
        public async Task<JsonResult> Active()
        {
            var result = await categoriaService.Ativas();
            if (result.Count() != 0)
                return Json(result.ToList());
            else
                return Json(null);
        }
    } 
}