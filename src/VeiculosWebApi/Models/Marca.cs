using System.Collections.Generic;
using System.Linq;

namespace VeiculosWebApi.Models
{
    public class Marca : EntityBaseWithName<Marca>
    {
        public bool Principal { get; set; }

        public string Categoria { get; set; }

        public override IEnumerable<Marca> ActiveTrue(IEnumerable<Marca> marcas)
        {
            return marcas.Where(x => x.Active);
        }

        public override Marca PorNome(IEnumerable<Marca> marcas, string nome)
        {
            return marcas.Where(x => x.Name == nome).FirstOrDefault();
        }

        public IEnumerable<Marca> PorCategoria(IEnumerable<Marca> marcas, string categoria)
        {
            return marcas.Where(marca => marca.Categoria == categoria);
        }

        public Marca PorCategoriaENome(IEnumerable<Marca> marca, string categoria, string nome)
        {
            return marca.Where(x => (x.Categoria == categoria) && (x.Name == nome)).FirstOrDefault();
        }
    }
}
