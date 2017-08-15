using System.Collections.Generic;
using System.Linq;

namespace VeiculosWebApi.Models
{
    public class Categoria : EntityBaseWithName<Categoria>
    {
        public override IEnumerable<Categoria> ActiveTrue(IEnumerable<Categoria> categorias)
        {
            return categorias.Where(x => x.Active);
        }

        public override Categoria PorNome(IEnumerable<Categoria> categorias, string nome)
        {
            return categorias.Where(x => x.Name == nome).FirstOrDefault();
        }
    }
}