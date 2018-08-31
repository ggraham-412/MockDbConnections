using Npgsql;
using System.Data;

namespace MockDbConnections
{
    /// <summary>
    ///     PostgreSQL connection factory
    /// </summary>
    public class NPGConnectionFactory : IDbConnectionFactory
    {
        public IDbCommand CreateCommand(string sqlText, IDbConnection cxn)
        {
            return new NpgsqlCommand(sqlText, (NpgsqlConnection)cxn);
        }

        public IDbConnection CreateConnection(string cxnString)
        {
            return new NpgsqlConnection(cxnString);
        }
    }
}
