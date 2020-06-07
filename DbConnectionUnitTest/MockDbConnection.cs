﻿using System;
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

        private string _database;
        public override string Database => _database;

        private ConnectionState _state;
        public override ConnectionState State => _state;

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return new MockDbTransaction();
        }

        public override void ChangeDatabase(string databaseName)
        {
            _database = databaseName;
        }

        public override void Close()
        {
            _state = ConnectionState.Closed;
        }

        protected override DbCommand CreateDbCommand()
        {
            if (_returnValues.Length < _currentCommandIndex - 1)
            {
                throw new ArgumentException("Require ReturnValue for DbCommand, please use InjectReturnValues method.");
            }

            var cmd = new MockDbCommand() { Connection = this, ReturnValue = _returnValues[_currentCommandIndex] };
            _currentCommandIndex++;

            return cmd;
        }

        public override void Open()
        {
            _state = ConnectionState.Open;
        }

        public override string DataSource => throw new NotImplementedException();

        public override string ServerVersion => throw new NotImplementedException();
        #endregion

        private object[] _returnValues;
        private int _currentCommandIndex;

        /// <summary>
        /// Clean
        /// </summary>
        public void Clean()
        {
            _returnValues = null;
            _currentCommandIndex = 0;
            this.Dispose();
        }

        /// <summary>
        /// Inject return values
        /// </summary>
        /// <param name="returnValues"></param>
        public void InjectReturnValues(params object[] returnValues)
        {
            _returnValues = returnValues;
        }
    }
}
