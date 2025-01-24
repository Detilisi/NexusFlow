using Dapper;
using NexusFlow.PublicApi.Models;
using System.Data;

namespace NexusFlow.PublicApi.Data.Repositories;

public class PersonRepository
{
    private readonly DataAccess _dataAccess;

    public PersonRepository(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<int> CreatePersonAsync(Person person)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@id_number", person.IdNumber, DbType.String);
        parameters.Add("@name", person.Name, DbType.String);
        parameters.Add("@surname", person.Surname, DbType.String);

        var result = await connection.ExecuteAsync("CreatePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<int> UpdatePersonAsync(Person person)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@code", person.Code, DbType.Int32);
        parameters.Add("@id_number", person.IdNumber, DbType.String);
        parameters.Add("@name", person.Name, DbType.String);
        parameters.Add("@surname", person.Surname, DbType.String);

        var result = await connection.ExecuteAsync("UpdatePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<int> DeletePersonAsync(int code)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@code", code, DbType.Int32);

        var result = await connection.ExecuteAsync("DeletePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<Person?> GetPersonByCriteriaAsync(string? idNumber = null, string? surname = null, string? accountNumber = null)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();

        // Add parameters if they are provided
        parameters.Add("@id_number", idNumber, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("@surname", surname, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("@account_number", accountNumber, DbType.String, direction: ParameterDirection.Input);

        // Query the stored procedure
        var result = await connection.QueryFirstOrDefaultAsync<Person>(
            "GetPersonByCriteria",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }


    public async Task<IEnumerable<Person>> GetAllPersonsAsync()
    {
        using var connection = _dataAccess.GetDbConnection();
        var result = await connection.QueryAsync<Person>("GetAllPersons", commandType: CommandType.StoredProcedure);
        return result;
    }
}
