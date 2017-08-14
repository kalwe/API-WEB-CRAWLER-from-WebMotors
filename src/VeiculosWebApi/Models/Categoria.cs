using System.Collections.Generic;
using System.Linq;

namespace VeiculosWebApi.Models
{
    public class Categoria : EntityBaseWithName
    {
        public IEnumerable<Categoria> Ativas(IEnumerable<Categoria> categorias)
        {
            return categorias.Where(x => x.Active);
        }

        public Categoria PorNome(IEnumerable<Categoria> categorias, string nome)
        {
            return categorias.Where(x => x.Name == nome).FirstOrDefault();
        }
    }
}