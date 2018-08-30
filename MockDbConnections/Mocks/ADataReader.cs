using System;
using System.Collections.Generic;
using System.Data;

namespace MockDbConnections.Mocks
{
    /// <summary>
    ///     Partial implementation of an IDataReader that supports a settable RowSet.
    /// </summary>
    public abstract class ADataReader : IDataReader
    {
        // The settable RowSet
        public List<object[]> RowSet { get; set; } = new List<object[]>();

        // Advancing the row
        public int CurrentRow { get; set; } = -1;
        public bool Read()
        {
            return ++CurrentRow < RowSet.Count;
        }

        // Accessor methods
        public object this[int i] => RowSet[CurrentRow][i];

        public bool GetBoolean(int i)
        {
            return (bool)this[i];
        }

        public byte GetByte(int i)
        {
            return (byte)this[i];
        }

        public char GetChar(int i)
        {
            return (char)this[i];
        }

        public DateTime GetDateTime(int i)
        {
            return (DateTime)this[i];
        }

        public decimal GetDecimal(int i)
        {
            return (decimal)this[i];
        }

        public double GetDouble(int i)
        {
            return (double)this[i];
        }

        public float GetFloat(int i)
        {
            return (float)this[i];
        }

        public short GetInt16(int i)
        {
            return (short)this[i];
        }

        public int GetInt32(int i)
        {
            return (int)this[i];
        }

        public long GetInt64(int i)
        {
            return (long)this[i];
        }

        public string GetString(int i)
        {
            return (string)this[i];
        }

        public object GetValue(int i)
        {
            return this[i];
        }

        public bool IsDBNull(int i)
        {
            return this[i] == null || this[i] == DBNull.Value;
        }

        // Unimplemented methods 

        public abstract int Depth { get; }
        public abstract bool IsClosed { get; }
        public abstract int RecordsAffected { get; }
        public abstract int FieldCount { get; }
        public abstract object this[string name] { get; }
        public abstract void Close();
        public abstract DataTable GetSchemaTable();
        public abstract bool NextResult();
        public abstract void Dispose();
        public abstract string GetName(int i);
        public abstract string GetDataTypeName(int i);
        public abstract Type GetFieldType(int i);
        public abstract int GetValues(object[] values);
        public abstract int GetOrdinal(string name);
        public abstract long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length);
        public abstract long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length);
        public abstract Guid GetGuid(int i);
        public abstract IDataReader GetData(int i);
    }
}
