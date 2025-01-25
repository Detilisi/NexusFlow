using Microsoft.Data.SqlClient;
using System.Data;

namespace NexusFlow.PublicApi.Data;

public class DataAccess
{
    private readonly string _connectionString;

    public DataAccess(string connectionString) => _connectionString = connectionString;

    public IDbConnection GetDbConnection() => new SqlConnection(_connectionString);
}
