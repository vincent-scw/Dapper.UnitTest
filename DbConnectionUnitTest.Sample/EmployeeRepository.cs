using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbConnectionUnitTest.Sample
{
    class EmployeeRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        private static readonly string CommandString = "SELECT Id, Name FROM dbo.Employee WHERE ID=@id";

        public EmployeeRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public dynamic GetOne(int id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = CommandString;
                var p = cmd.CreateParameter();
                p.ParameterName = "@id";
                p.DbType = System.Data.DbType.Int32;
                p.Value = id;

                cmd.Parameters.Add(p);
                var reader = cmd.ExecuteReader();

                object obj = null;
                while (reader.Read())
                {
                    obj = new { Id = reader[0], Name = reader[1] };
                    break;
                }
                return obj;
            }
        }

        public dynamic GetOneWithDapper(int id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var obj = conn.QueryFirstOrDefault<dynamic>(CommandString, new { id });

                return obj;
            }
        }

        public async Task<dynamic> GetOneWithDapperAsync(int id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var obj = await conn.QueryFirstOrDefaultAsync<dynamic>(CommandString, new { id });

                return obj;
            }
        }
    }
}
