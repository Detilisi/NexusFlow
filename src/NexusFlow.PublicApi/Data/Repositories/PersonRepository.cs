using Dapper;
using NexusFlow.PublicApi.Models;
using System.Data;

namespace NexusFlow.PublicApi.Data.Repositories;

public class PersonRepository
{
    private readonly DataAccess _dataAccess;

    public PersonRepository()
    {
        _dataAccess = new DataAccess();
    }

    public async Task<int> CreatePersonAsync(Person person)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@IDNumber", person.IdNumber, DbType.String);
        parameters.Add("@FirstName", person.Name, DbType.String);
        parameters.Add("@LastName", person.Surname, DbType.String);

        var result = await connection.ExecuteAsync("CreatePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<int> UpdatePersonAsync(Person person)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@PersonCode", person.Code, DbType.Int32);
        parameters.Add("@IDNumber", person.IdNumber, DbType.String);
        parameters.Add("@FirstName", person.Name, DbType.String);
        parameters.Add("@LastName", person.Surname, DbType.String);

        var result = await connection.ExecuteAsync("UpdatePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<int> DeletePersonAsync(int personCode)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@PersonCode", personCode, DbType.Int32);

        var result = await connection.ExecuteAsync("DeletePerson", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<Person> GetPersonByIDAsync(string idNumber)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@IDNumber", idNumber, DbType.String);

        var result = await connection.QueryFirstOrDefaultAsync<Person>("GetPersonByID", parameters, commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<IEnumerable<Person>> GetAllPersonsAsync()
    {
        using var connection = _dataAccess.GetDbConnection();
        var result = await connection.QueryAsync<Person>("GetAllPersons", commandType: CommandType.StoredProcedure);
        return result;
    }
}
