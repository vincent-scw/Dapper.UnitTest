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
            if (ExecutedDbCommands == null)
                ExecutedDbCommands = new List<MockDbCommand>();

            if (_returnValues.Length < ExecutedDbCommands.Count)
            {
                throw new ArgumentException("Have more DbCommands than ReturnValues, please inject values via InjectReturnValues.");
            }

            var cmd = new MockDbCommand() { Connection = this, ReturnValue = _returnValues[ExecutedDbCommands.Count] };
            
            ExecutedDbCommands.Add(cmd);

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

        /// <summary>
        /// Clean
        /// </summary>
        public void Clean()
        {
            _returnValues = null;
            ExecutedDbCommands = null;
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

        public List<MockDbCommand> ExecutedDbCommands { get; private set; }
    }
}
