using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DbConnectionUnitTest
{
    public class MockDbConnection : DbConnection
    {
        #region IDbConnection
        public override string ConnectionString { get; set; }

        public override int ConnectionTimeout { get; }

        public override string Database { get; }

        public override ConnectionState State { get; }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new MockDbTransaction();
        }

        public override void ChangeDatabase(string databaseName)
        {
        }

        public override void Close()
        {
        }

        protected override DbCommand CreateDbCommand()
        {
            return new MockDbCommand() { Connection = this };
        }

        public override void Open()
        {
        }
        #endregion

        private object _returnValue;
        /// <summary>
        /// ReturnValue of DbCommand
        /// </summary>
        public object ReturnValue 
        { 
            get { return _returnValue; }
            set
            {
                if (value is IEnumerable)
                {
                    throw new ArgumentException("ReturnValue shouldn't be enumerable. One object is expected.");
                }

                _returnValue = value;
            }
        }

        public override string DataSource => throw new NotImplementedException();

        public override string ServerVersion => throw new NotImplementedException();

        public void Clean()
        {
            ReturnValue = null;
            this.Dispose();
        }
    }
}
