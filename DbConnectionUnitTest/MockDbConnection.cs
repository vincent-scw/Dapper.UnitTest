using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DbConnectionUnitTest
{
    public class MockDbConnection : IDbConnection
    {
        #region IDbConnection
        public string ConnectionString { get; set; }

        public int ConnectionTimeout { get; set; }

        public string Database { get; set; }

        public ConnectionState State { get; set; }

        public IDbTransaction BeginTransaction()
        {
            return new MockDbTransaction { Connection = this };
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return new MockDbTransaction { Connection = this, IsolationLevel = il };
        }

        public void ChangeDatabase(string databaseName)
        {
            Database = databaseName;
        }

        public void Close()
        {
            State = ConnectionState.Closed;
        }

        public IDbCommand CreateCommand()
        {
            return new MockDbCommand(this);
        }

        public void Dispose()
        {
            Close();
        }

        public void Open()
        {
            State = ConnectionState.Open;
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

        public void Clean()
        {
            ReturnValue = null;
            this.Dispose();
        }
    }
}
