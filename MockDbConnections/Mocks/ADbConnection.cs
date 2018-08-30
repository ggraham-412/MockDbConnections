using System.Data;

namespace MockDbConnections.Mocks
{
    /// <summary>
    ///     Partial implementation of an IDbConnection that supports ADbCommand and ADataReader.
    /// </summary>
    public abstract class ADbConnection : IDbConnection
    {
        // implemented properties
        public string ConnectionString { get; set; }

        // unimplemented methods
        public abstract IDbCommand CreateCommand();  // This one will be mocked
        public abstract int ConnectionTimeout { get; }
        public abstract string Database { get; }
        public abstract ConnectionState State { get; }

        public abstract IDbTransaction BeginTransaction();
        public abstract IDbTransaction BeginTransaction(IsolationLevel il);
        public abstract void ChangeDatabase(string databaseName);
        public abstract void Close();
        public abstract void Dispose();
        public abstract void Open();
    }
}
