using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DbConnectionUnitTest.Sample
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
