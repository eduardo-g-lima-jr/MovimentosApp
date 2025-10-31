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
            using var conn = _connFactory.CreateConnection();

            const string sql = @"
                SELECT  
                    COD_PRODUTO, 
                    DES_PRODUTO, 
                    STA_STATUS
                FROM dbo.PRODUTO
                ORDER BY DES_PRODUTO";

            return conn.Query<Produto>(sql);
        }

        public IEnumerable<ProdutoCosif> GetCosifsByProduto(string codProduto)
        {
            using var conn = _connFactory.CreateConnection();

            const string sql = @"
                SELECT  
                    COD_PRODUTO, 
                    COD_COSIF, 
                    COD_CLASSIFICACAO
                FROM dbo.PRODUTO_COSIF
                WHERE COD_PRODUTO = @codProduto
                  AND STA_STATUS = 'A'";

            return conn.Query<ProdutoCosif>(sql, new { codProduto });
        }

        public long GetNextNumeroLancamento(int mes, int ano)
        {
            using var conn = _connFactory.CreateConnection();

            const string sql = @"
                SELECT  
                    ISNULL(MAX(NUM_LANCAMENTO), 0) + 1
                FROM dbo.MOVIMENTO_MANUAL
                WHERE DAT_MES = @mes 
                  AND DAT_ANO = @ano";

            var nextValue = conn.QueryFirstOrDefault<long?>(sql, new { mes, ano });
            return nextValue ?? 1;
        }

        public void InsertMovimento(MovimentoManual movimento)
        {
            using var conn = _connFactory.CreateConnection();

            const string sql = @"
                INSERT INTO dbo.MOVIMENTO_MANUAL
                    (DAT_MES, DAT_ANO, NUM_LANCAMENTO, COD_PRODUTO, COD_COSIF, 
                     VAL_VALOR, DES_DESCRICAO, DAT_MOVIMENTO, COD_USUARIO)
                VALUES
                    (@DAT_MES, @DAT_ANO, @NUM_LANCAMENTO, @COD_PRODUTO, @COD_COSIF, 
                     @VAL_VALOR, @DES_DESCRICAO, @DAT_MOVIMENTO, @COD_USUARIO)";

            conn.Execute(sql, movimento);
        }

        public IEnumerable<MovimentoManual> GetMovimentosPorMesAno(int mes, int ano)
        {
            using var conn = _connFactory.CreateConnection();

            const string sql = @"
                SELECT  
                    M.DAT_MES,
                    M.DAT_ANO,
                    M.COD_PRODUTO,
                    P.DES_PRODUTO,
                    M.NUM_LANCAMENTO,
                    M.DES_DESCRICAO,
                    M.VAL_VALOR
                FROM dbo.MOVIMENTO_MANUAL AS M
                INNER JOIN dbo.PRODUTO AS P 
                    ON P.COD_PRODUTO = M.COD_PRODUTO
                WHERE M.DAT_MES = @mes 
                  AND M.DAT_ANO = @ano
                ORDER BY M.NUM_LANCAMENTO";

            return conn.Query<MovimentoManual>(sql, new { mes, ano });
        }
    }
}
