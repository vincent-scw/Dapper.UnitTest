using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DbConnectionUnitTest
{
    class MockDbTransaction : DbTransaction
    {
        public override IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;

        protected override DbConnection DbConnection => throw new NotImplementedException();

        public override void Commit()
        {
            
        }

        public override void Rollback()
        {
            
        }
    }
}
