using System.Collections.Generic;
using System.Linq;

namespace VeiculosWebApi.Models
{
    public class Marca : EntityBaseWithName
    {
        public bool Principal { get; set; }

        public string Categoria { get; set; }

        public IEnumerable<Marca> Ativas(IEnumerable<Marca> marcas)
        {
            return marcas.Where(x => x.Active);
        }

        public IEnumerable<Marca> PorCategoria(IEnumerable<Marca> marcas, string categoria)
        {
            return marcas.Where(marca => marca.Categoria == categoria);
        }

        public Marca PorNome(IEnumerable<Marca> marca, string categoria, string nome)
        {
            return marca.Where(x => (x.Categoria == categoria) && (x.Name == nome)).FirstOrDefault();
        }
    }
}
