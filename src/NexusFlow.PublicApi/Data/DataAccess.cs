using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;

namespace NexusFlow.PublicApi.Data;

public class DataAccess
{
    private SqlConnection _connection;

    public DataAccess()
    {
        _connection = new SqlConnection("Server=localhost;Database=NexusFlow;Trusted_Connection=True;");
    }
}
