using System.Security.Cryptography;

namespace NexusFlow.PublicApi.Auth
{
    public class PasswordHasherService
    {
        private const int SaltSize = 16; // 128-bit salt
        private const int KeySize = 32;  // 256-bit key
        private const int Iterations = 10000; // Number of hash iterations
        private const char Delimiter = '|'; // Used to separate salt and hash in the stored password

        /// <summary>
        /// Hashes a password with a generated salt.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>A hashed password including the salt.</returns>
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or whitespace.", nameof(password));

            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var hash = HashPasswordWithSalt(password, salt);

            return $"{Convert.ToBase64String(salt)}{Delimiter}{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Verifies a password against a stored hash.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="storedPassword">The stored password hash including the salt.</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        public bool VerifyPassword(string password, string storedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedPassword))
                return false;

            var parts = storedPassword.Split(Delimiter);
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var computedHash = HashPasswordWithSalt(password, salt);

            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }

        /// <summary>
        /// Hashes a password with a specific salt using PBKDF2.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <param name="salt">The salt to use.</param>
        /// <returns>The hashed password as a byte array.</returns>
        private byte[] HashPasswordWithSalt(string password, byte[] salt)
        {
            using var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            return deriveBytes.GetBytes(KeySize);
        }
    }
}
