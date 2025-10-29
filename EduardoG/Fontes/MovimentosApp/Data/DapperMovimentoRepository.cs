using Dapper;
using MovimentosApp.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MovimentosApp.Data
{
    public class DapperMovimentoRepository
    {
        private readonly IDbConnectionFactory _connFactory;
        public DapperMovimentoRepository(IDbConnectionFactory connFactory)
        {
            _connFactory = connFactory;
        }

        public IEnumerable<Produto> GetAllProdutos()
        {
            using(var c = _connFactory.CreateConnection())
            {
                return c.Query<Produto>("SELECT COD_PRODUTO, DES_PRODUTO, STA_STATUS FROM dbo.PRODUTO ORDER BY DES_PRODUTO");
            }
        }

        public IEnumerable<ProdutoCosif> GetCosifsByProduto(string codProduto)
        {
            using(var c = _connFactory.CreateConnection())
            {
                var sql = "SELECT COD_PRODUTO, COD_COSIF, COD_CLASSIFICACAO, STA_STATUS FROM dbo.PRODUTO_COSIF WHERE COD_PRODUTO = @p ORDER BY COD_COSIF";
                return c.Query<ProdutoCosif>(sql, new { p = codProduto });
            }
        }

        public long GetNextNumeroLancamento(int mes, int ano)
        {
            using(var c = _connFactory.CreateConnection())
            {
                var val = c.QueryFirstOrDefault<long?>(
                    "SELECT ISNULL(MAX(NUM_LANCAMENTO), 0) + 1 FROM dbo.MOVIMENTO_MANUAL WHERE DAT_MES = @m AND DAT_ANO = @a",
                    new { m = mes, a = ano });
                return val ?? 1;
            }
        }

        public void InsertMovimento(MovimentoManual m)
        {
            using(var c = _connFactory.CreateConnection())
            {
                var sql = @"INSERT INTO dbo.MOVIMENTO_MANUAL
                    (DAT_MES, DAT_ANO, NUM_LANCAMENTO, COD_PRODUTO, COD_COSIF, VAL_VALOR, DES_DESCRICAO, DAT_MOVIMENTO, COD_USUARIO)
                    VALUES (@DAT_MES, @DAT_ANO, @NUM_LANCAMENTO, @COD_PRODUTO, @COD_COSIF, @VAL_VALOR, @DES_DESCRICAO, @DAT_MOVIMENTO, @COD_USUARIO)";
                c.Execute(sql, m);
            }
        }

        public IEnumerable<dynamic> GetMovimentosPorMesAno(int mes, int ano)
        {
            using(var c = _connFactory.CreateConnection())
            {
                var p = new DynamicParameters();
                p.Add("@Mes", mes);
                p.Add("@Ano", ano);
                return c.Query("dbo.usp_GetMovimentosPorMesAno", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
