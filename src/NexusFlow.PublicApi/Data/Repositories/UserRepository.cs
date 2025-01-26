using Dapper;
using NexusFlow.PublicApi.Auth;
using NexusFlow.PublicApi.Models;
using System.Data;

namespace NexusFlow.PublicApi.Data.Repositories
{
    public class UserRepository
    {
        private readonly DataAccess _dataAccess;
        private readonly PasswordHasherService _passwordHasher;
        public UserRepository(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _passwordHasher = new PasswordHasherService();
        }

        public async Task<User?> LoginAsync(User loginUser)
        {
            using var connection = _dataAccess.GetDbConnection();

            var parameters = new DynamicParameters();
            parameters.Add(nameof(User.Email), loginUser.Email, DbType.String);

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "LoginUser",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (user == null || !_passwordHasher.VerifyPassword(loginUser.PasswordHash, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }
}
