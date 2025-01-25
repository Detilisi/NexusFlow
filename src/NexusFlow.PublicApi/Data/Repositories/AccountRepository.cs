using Dapper;
using NexusFlow.PublicApi.Models;
using System.Data;

namespace NexusFlow.PublicApi.Data.Repositories;

public class AccountRepository
{
    private readonly DataAccess _dataAccess;

    public AccountRepository(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<int> CreateAccountAsync(Account account)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(Account.PersonCode)}", account.PersonCode, DbType.Int32);
        parameters.Add($"@{nameof(Account.AccountNumber)}", account.AccountNumber, DbType.String);

        var result = await connection.ExecuteAsync("CreateAccount", parameters, commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<int> UpdateAccountDetailsAsync(Account account)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(Account.Code)}", account.Code, DbType.Int32);
        parameters.Add($"@{nameof(Account.AccountNumber)}", account.AccountNumber, DbType.String);

        var result = await connection.ExecuteAsync(
            "UpdateAccountNumber",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }
    public async Task<int> UpdateAccountStatus(int accountCode, AccountStatus accountStatus)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(Account.Code)}", accountCode, DbType.Int32);
        parameters.Add($"@{nameof(Account.StatusCode)}", (int)accountStatus, DbType.Int32);

        var result = await connection.ExecuteAsync(
            "UpdateAccountStatus",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<IEnumerable<Account>> GetAccounts(int personCode = -1, int accountCode = -1)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@PersonCode", personCode);
        parameters.Add("@Code", accountCode);


        var result = await connection.QueryAsync(
            "GetAccounts",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        var accounts = result.Select(row => new Account
        {
            Code = row.code,
            PersonCode = row.person_code,
            AccountNumber = row.account_number,
            OutstandingBalance = row.outstanding_balance,
            StatusCode = row.status_code
        });

        return accounts;
    }

    public async Task<int> DeleteAccountAsync(int accountCode)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(Account.Code)}", accountCode, DbType.Int32);

        var result = await connection.ExecuteAsync(
            "DeleteAccount",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }
}
