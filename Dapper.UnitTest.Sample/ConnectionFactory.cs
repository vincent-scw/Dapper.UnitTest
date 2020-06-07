using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Dapper.UnitTest.Sample
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
