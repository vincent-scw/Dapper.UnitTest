using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DbConnectionUnitTest
{
    public class MockDbCommand : DbCommand
    {
        public MockDbCommand()
        {
            _dbParameterCollection = new MockDbParameterCollection();
        }

        public object ReturnValue { get; set; }

        #region DbCommand
        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }

        private MockDbParameterCollection _dbParameterCollection;
        protected override DbParameterCollection DbParameterCollection => _dbParameterCollection;

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel()
        {
            
        }

        public override int ExecuteNonQuery()
        {
            return (int)ReturnValue;
        }

        public override object ExecuteScalar()
        {
            return ReturnValue;
        }

        public override void Prepare()
        {
            
        }

        protected override DbParameter CreateDbParameter()
        {
            return new MockDbParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return new MockDbDataReader() { ReturnValue = ReturnValue };
        }
        #endregion
    }
}
