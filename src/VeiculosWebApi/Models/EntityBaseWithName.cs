using System.Collections.Generic;
using System.Linq;

namespace VeiculosWebApi.Models
{
    public abstract class EntityBaseWithName<TEntity> : EntityBase<TEntity>
    {
        public string Name { get; set; }

        public abstract TEntity PorNome(IEnumerable<TEntity> entities, string nome);
    }
}