using System.Data;

namespace MockDbConnections.Mocks
{
    /// <summary>
    ///     Partial implementation of an IDbCommand that supports settable results.
    /// </summary>
    public abstract class ADbCommand : IDbCommand
    {
        // The settable results
        public int NonQueryResult { get; set; }
        public object ScalarResult { get; set; }
        public IDataReader ReaderResult { get; set; }

        // Implemented methods
        public IDbConnection Connection { get; set; }
        public string CommandText { get; set; }
        public int ExecuteNonQuery()
        {
            return NonQueryResult;
        }
        public IDataReader ExecuteReader()
        {
            return ReaderResult;
        }
        public virtual IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return ReaderResult;
        }
        public object ExecuteScalar()
        {
            return ScalarResult;
        }

        // Unimplemented methods
        public abstract CommandType CommandType { get; set; }
        public abstract IDbTransaction Transaction { get; set; }
        public abstract int CommandTimeout { get; set; }
        public abstract IDataParameterCollection Parameters { get; }
        public abstract UpdateRowSource UpdatedRowSource { get; set; }
        public abstract void Cancel();
        public abstract IDbDataParameter CreateParameter();
        public abstract void Dispose();
        public abstract void Prepare();
    }
}
