using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbConnectionUnitTest.Sample
{
    class EmployeeRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        private static readonly string CommandString_ONE = "SELECT Id, Name, Age, Joined FROM dbo.Employee WHERE ID=@id";

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
                cmd.CommandText = CommandString_ONE;
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

                var obj = conn.QueryFirstOrDefault<EmployeeModel>(CommandString_ONE, new { id });

                return obj;
            }
        }

        public async Task<EmployeeModel> GetOneWithDapperAsync(Guid id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var obj = await conn.QueryFirstOrDefaultAsync<EmployeeModel>(CommandString_ONE, new { id });

                return obj;
            }
        }

        public async Task<List<EmployeeModel>> GetManyWithDapperAsync()
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var employees = await conn.QueryAsync<EmployeeModel>("SELECT Id, Name, Age, Joined FROM dbo.Employee");

                return employees.ToList();
            }
        }

        public async Task<int> GetScalarWithDapperAsync()
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                var count = await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.Employee");

                return count;
            }
        }

        public async Task<EmployeeModel> ComplicateCaseAsync(Guid id)
        {
            using (var conn = _connectionFactory.GetConnection())
            {
                conn.Open();

                using (var tran = conn.BeginTransaction())
                {
                    var exists = await conn.ExecuteScalarAsync<bool>("SELECT 1 WHERE Id=@id", new { id });

                    if (exists)
                    {
                        await conn.ExecuteAsync("UPDATE dbo.Employee SET Name='UpdatedName' WHERE Id=@id", new { id });
                    }

                    return await conn.QueryFirstOrDefaultAsync<EmployeeModel>(CommandString_ONE, new { id });
                }
            }
        }
    }
}
