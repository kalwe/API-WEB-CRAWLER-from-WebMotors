using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Models;

namespace VeiculosWebApi.Repositories
{
    public class ModeloRepository : RepositoryBase<Modelo>, IModeloRepository
    {
        public ModeloRepository(IVeiculosDbContext db)
            : base(db)
        { }
    }
}