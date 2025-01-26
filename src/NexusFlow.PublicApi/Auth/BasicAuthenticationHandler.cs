using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NexusFlow.PublicApi.Data.Repositories;
using NexusFlow.PublicApi.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace NexusFlow.PublicApi.Auth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UserRepository _userRepo;
        private readonly PasswordHasherService _passwordHasher;
        public BasicAuthenticationHandler
        (
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserRepository dataAccess
        ) : base(options, logger, encoder, clock)
        {
            _userRepo = dataAccess;
            _passwordHasher = new PasswordHasherService();
         }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];

                // Validate user
                var user = await _userRepo.LoginAsync(new User { Email = username, Password = password });
                if (user == null)
                {
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }
                    
                /*// Verify password
                var hashedPassword = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(password)));
                bool isValid = _passwordHasher.VerifyPassword(hashedPassword, user.Password);
                
                if (!isValid) return AuthenticateResult.Fail("Invalid Username or Password");*/

                // Create claims
                var claims = new[] {
                    new Claim(ClaimTypes.Name, user.Email)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
        }
    }
}
