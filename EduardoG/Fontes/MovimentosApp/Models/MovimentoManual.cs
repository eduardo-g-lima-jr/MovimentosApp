using System;

namespace MovimentosApp.Models
{
    public class MovimentoManual
    {
        public int DAT_MES { get; set; }
        public int DAT_ANO { get; set; }
        public long NUM_LANCAMENTO { get; set; }
        public string COD_PRODUTO { get; set; }
        public string COD_COSIF { get; set; }
        public decimal VAL_VALOR { get; set; }
        public string DES_DESCRICAO { get; set; }
        public DateTime DAT_MOVIMENTO { get; set; }
        public string COD_USUARIO { get; set; }
    }
}
