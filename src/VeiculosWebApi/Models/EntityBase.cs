using System.Collections.Generic;

namespace VeiculosWebApi.Models
{
    // Class base for entities properties
    public abstract class EntityBase<TEntity>
    {
        public string Id { get; set;}

        public string CreateTimeStamp { get; set;}

        public string ModificationTimeStamp { get; set; }

        public bool Active { get; set;}

        public abstract IEnumerable<TEntity> ActiveTrue(IEnumerable<TEntity> entities);
    }
}