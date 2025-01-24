﻿using Dapper;
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
        parameters.Add("@person_code", account.PersonCode, DbType.Int32);
        parameters.Add("@account_number", account.AccountNumber, DbType.String);

        var result = await connection.ExecuteAsync("CreateAccount", parameters, commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<int> UpdateAccountDetailsAsync(Account account)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@account_code", account.Code, DbType.Int32);
        parameters.Add("@new_account_number", account.AccountNumber, DbType.String);

        var result = await connection.ExecuteAsync(
            "UpdateAccountNumber",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }
    public async Task<int> CloseAccountAsync(int accountCode, AccountStatus accountStatus)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@account_code", accountCode, DbType.Int32);
        parameters.Add("@new_status_code", accountStatus, DbType.Int32);

        var result = await connection.ExecuteAsync(
            "UpdateAccountStatus",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<Account?> GetAccountByCodeAsync(int accountCode)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@account_code", accountCode, DbType.Int32);

        var result = await connection.QueryFirstOrDefaultAsync<Account>(
            "GetAccountByCode",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<IEnumerable<Account>> GetAccountsForPersonAsync(int personCode, int? accountCode)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@person_code", personCode, DbType.Int32);
        parameters.Add("@account_code", accountCode, DbType.Int32);

        var result = await connection.QueryAsync<Account>(
            "GetAccountsForPerson",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<int> DeleteAccountAsync(int accountCode)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@account_code", accountCode, DbType.Int32);

        var result = await connection.ExecuteAsync(
            "DeleteAccount",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }
}
