using System.Collections.Generic;

namespace MovimentosApp.Models
{
    public class MovimentoViewModel
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public long ProximoNumeroLancamento { get; set; }
        public IEnumerable<Produto> Produtos { get; set; }
        public IEnumerable<ProdutoCosif> Cosifs { get; set; }
    }
}
