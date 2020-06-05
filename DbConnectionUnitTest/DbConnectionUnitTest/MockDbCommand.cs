using System;
using System.Data;

namespace DbConnectionUnitTest
{
    class MockDbCommand : IDbCommand
    {
        public int ExecuteCount { get; private set; }

        public DataTable ReturnValues { get; set; }
        public object ScalarValue { get; set; }

        public MockDbCommand()
        {
            Parameters = new MockDbDataParameterCollection();
            ReturnValues = new DataTable(); // by default, add an empty table
            ScalarValue = new object(); // by default, add an empty object
        }

        #region Implement IDbCommand
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        public string CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }

        public IDataParameterCollection Parameters { get; private set; }

        public UpdateRowSource UpdatedRowSource { get; set; }

        public void Cancel()
        {

        }

        public IDbDataParameter CreateParameter()
        {
            //var p = new SqlParameter();
            //return p;
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public int ExecuteNonQuery()
        {
            ExecuteCount++;
            return 1;
        }

        public IDataReader ExecuteReader()
        {
            ExecuteCount++;
            return new DataTableReader(ReturnValues);
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            ExecuteCount++;
            return new DataTableReader(ReturnValues);
        }

        public object ExecuteScalar()
        {
            ExecuteCount++;
            return ScalarValue;
        }

        public void Prepare()
        {

        }
        #endregion
    }
}