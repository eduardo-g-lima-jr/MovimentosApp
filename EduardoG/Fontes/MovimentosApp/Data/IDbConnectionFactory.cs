using System.Data;

namespace MovimentosApp.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
