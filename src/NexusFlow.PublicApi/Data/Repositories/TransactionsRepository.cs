using Dapper;
using NexusFlow.PublicApi.Models;
using System.Data;

namespace NexusFlow.PublicApi.Data.Repositories;

public class TransactionsRepository
{
    private readonly DataAccess _dataAccess;

    public TransactionsRepository(DataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task<int> CreateTransactionAsync(Transaction transaction)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(Transaction.AccountCode)}", transaction.AccountCode, DbType.Int32);
        parameters.Add($"@{nameof(Transaction.Amount)}", transaction.Amount, DbType.Currency);
        parameters.Add($"@{nameof(Transaction.Description)}", transaction.Description, DbType.String);
        parameters.Add($"@{nameof(Transaction.TransactionDate)}", transaction.TransactionDate, DbType.Date);
        
        var result = await connection.ExecuteAsync("CreateTransaction", parameters, commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<int> UpdateTransactionAsync(Transaction transaction)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add($"@{nameof(Transaction.Code)}", transaction.Code, DbType.Int32);
        parameters.Add($"@{nameof(Transaction.TransactionDate)}", transaction.TransactionDate, DbType.DateTime);
        parameters.Add($"@{nameof(Transaction.Amount)}", transaction.Amount, DbType.Currency);
        parameters.Add($"@{nameof(Transaction.Description)}", transaction.Description, DbType.String);
        

        var result = await connection.ExecuteAsync(
            "UpdateTransaction",
            parameters,
            commandType: CommandType.StoredProcedure);

        return result;
    }

    public async Task<IEnumerable<Transaction>> GetTransactions(int accountCode = -1, int code = -1)
    {
        using var connection = _dataAccess.GetDbConnection();
        var parameters = new DynamicParameters();
        parameters.Add("@AccountCode", accountCode);
        parameters.Add("@Code", code);


        var result = await connection.QueryAsync(
            "GetTransactions",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        var transactions = result.Select(row => new Transaction
        {
            Code = row.code,
            AccountCode = row.account_code,
            Amount = row.amount,
            Description = row.description,
            CaptureDate = row.capture_date,
            TransactionDate = row.transaction_date
        });

        return transactions;
    }
}
