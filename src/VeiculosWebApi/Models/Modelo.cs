using System.Collections.Generic;
using System.Linq;

namespace VeiculosWebApi.Models
{
    public class Modelo : EntityBaseWithName
    {
        public string Marca { get; set; }

        public string Categoria { get; set; }

        public ICollection<string> Versao { get; set; }

        public IEnumerable<Modelo> Ativos(IEnumerable<Modelo> modelo)
        {
            return modelo.Where(x => x.Active);
        }

        public IEnumerable<Modelo> PorMarca(IEnumerable<Modelo> modelos, string categoria, string marca)
        {
            return modelos.Where(x => (x.Categoria == categoria) && (x.Marca == marca));
        }
    }
}