using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DbConnectionUnitTest
{
    class MockDbCommand : DbCommand
    {
        public MockDbCommand()
        {
        }

        public override string CommandText { get; set; }
        public override int CommandTimeout { get; set; }
        public override CommandType CommandType { get; set; }
        public override bool DesignTimeVisible { get; set; }
        public override UpdateRowSource UpdatedRowSource { get; set; }
        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection => new MockDbParameterCollection();

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel()
        {
            
        }

        public override int ExecuteNonQuery()
        {
            return 1;
        }

        public override object ExecuteScalar()
        {
            return "123";
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
            return new MockDbDataReader();
        }
    }
}
