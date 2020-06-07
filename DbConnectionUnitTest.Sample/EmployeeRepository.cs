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

        private static readonly string CommandString = "SELECT Id, Name, Age, Joined FROM dbo.Employee WHERE ID=@id";

        public EmployeeRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public EmployeeModel GetOne(Guid id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = CommandString;
                var p = cmd.CreateParameter();
                p.ParameterName = "@id";
                p.DbType = System.Data.DbType.Guid;
                p.Value = id;

                cmd.Parameters.Add(p);
                var reader = cmd.ExecuteReader();

                EmployeeModel obj = null;
                while (reader.Read())
                {
                    obj = new EmployeeModel 
                    { 
                        Id = (Guid)reader[0], 
                        Name = Convert.ToString(reader[1]), 
                        Age = Convert.ToInt32(reader[2]), 
                        Joined = Convert.ToDateTime(reader[3]) 
                    };
                    break;
                }
                return obj;
            }
        }

        public EmployeeModel GetOneWithDapper(Guid id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var obj = conn.QueryFirstOrDefault<EmployeeModel>(CommandString, new { id });

                return obj;
            }
        }

        public async Task<EmployeeModel> GetOneWithDapperAsync(Guid id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var obj = await conn.QueryFirstOrDefaultAsync<EmployeeModel>(CommandString, new { id });

                return obj;
            }
        }
    }
}
