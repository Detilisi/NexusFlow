using Dapper;
using NexusFlow.PublicApi.Models;
using System.Data;

namespace NexusFlow.PublicApi.Data.Repositories
{
    public class UserRepository
    {
        private readonly DataAccess _dataAccess;

        public UserRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<User?> LoginAsync(User loginUser)
        {
            using var connection = _dataAccess.GetDbConnection();

            var parameters = new DynamicParameters();
            parameters.Add(nameof(User.Email), loginUser.Email, DbType.String);
            parameters.Add(nameof(User.Password), loginUser.Password, DbType.String);

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "LoginUser",
                parameters,
                commandType: CommandType.StoredProcedure);

            return user;
        }

    }
}
