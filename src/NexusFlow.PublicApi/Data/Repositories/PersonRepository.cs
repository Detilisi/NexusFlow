using Dapper;
using NexusFlow.PublicApi.Models;
using System;
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
        parameters.Add($"@{nameof(Person.IdNumber)}", person.IdNumber, DbType.String);
        parameters.Add($"@{nameof(Person.Name)}", person.Name, DbType.String);
        parameters.Add($"@{nameof(Person.Surname)}", person.Surname, DbType.String);

        var result = await connection.ExecuteAsync("CreatePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }
    public async Task<int> DeletePersonAsync(int code)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(Person.Code)}", code, DbType.Int32);

        var result = await connection.ExecuteAsync("DeletePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<int> UpdatePersonAsync(Person person)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(Person.Code)}", person.Code, DbType.Int32);
        parameters.Add($"@{nameof(Person.IdNumber)}", person.IdNumber, DbType.String);
        parameters.Add($"@{nameof(Person.Name)}", person.Name, DbType.String);
        parameters.Add($"@{nameof(Person.Surname)}", person.Surname, DbType.String);

        var result = await connection.ExecuteAsync("UpdatePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<IEnumerable<Person>> GetPersons(int code=-1)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@Code", code, DbType.Int32, direction: ParameterDirection.Input);

        var result = await connection.QueryAsync<Person>("GetPersons", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }


    public async Task<Person?> GetPersonByCriteriaAsync(string searchTerm, string searchCriteria)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();

        // Add parameters if they are provided
        parameters.Add("@SearchTerm", searchTerm, DbType.String, direction: ParameterDirection.Input);
        parameters.Add("@SearchCriteria", searchCriteria, DbType.String, direction: ParameterDirection.Input);

        // Query the stored procedure
        var result = await connection.QueryFirstOrDefaultAsync<Person>(
            "GetPersonByCriteria",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }
}
