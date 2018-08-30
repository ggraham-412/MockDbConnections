using System.Data;

namespace MockDbConnections
{
    /// <summary>
    ///     Simplified factory interface for creating connections
    /// </summary>
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection(string cxnString);

        IDbCommand CreateCommand(string sqlText, IDbConnection cxn);
    }
}
