using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Repositories
{
    public class MarcaRepository : RepositoryBase<Marca>, IMarcaRepository
    {
        public MarcaRepository(IVeiculosDbContext db)
            : base(db)
        { }
    }
}
