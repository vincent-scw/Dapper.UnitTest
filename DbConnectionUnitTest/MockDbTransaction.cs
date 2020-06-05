using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DbConnectionUnitTest
{
    class MockDbTransaction : IDbTransaction
    {
        public IDbConnection Connection { get; set; }

        public IsolationLevel IsolationLevel { get; set; }

        public void Commit()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void Rollback()
        {
            
        }
    }
}
