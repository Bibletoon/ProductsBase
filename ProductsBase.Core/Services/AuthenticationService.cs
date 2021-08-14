using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductsBase.Data.Contexts;
using ProductsBase.Domain.Security.Hashing;
using ProductsBase.Domain.Security.Tokens;
using ProductsBase.Domain.Services.Communication;
using ProductsBase.Domain.Services.Interfaces;

namespace ProductsBase.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenHandler _tokenHandler;
        private readonly AppDbContext _dbContext;
        
        public AuthenticationService(IPasswordHasher passwordHasher, AppDbContext dbContext, ITokenHandler tokenHandler)
        {
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
            _tokenHandler = tokenHandler;
        }

        public async Task<TokenResponse> CreateAccessTokenAsync(string email, string password)
        {
            var user = await _dbContext.Users.Include(u=>u.Roles).FirstOrDefaultAsync(x=>x.Email == email);

            if (user is null || !_passwordHasher.PasswordMatches(password, user.Password))
            {
                return new TokenResponse("Wrong password");
            }

            var token = _tokenHandler.CreateAccessToken(user);

            return new TokenResponse(token);

        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, string userEmail)
        {
            var token = _tokenHandler.TakeRefreshToken(refreshToken);

            if (token == null)
            {
                return new TokenResponse("Invalid refresh token.");
            }

            if (token.IsExpired)
            {
                return new TokenResponse("Expired refresh token.");
            }

            var user = await _dbContext.Users.Include(u=>u.Roles).FirstOrDefaultAsync(x=>x.Email == userEmail);
            if (user == null)
            {
                return new TokenResponse(false, "Invalid refresh token.", null);
            }

            var accessToken = _tokenHandler.CreateAccessToken(user);
            return new TokenResponse(true, null, accessToken);
        }

        public void RevokeRefreshToken(string refreshToken)
        {
            _tokenHandler.RevokeRefreshToken(refreshToken);
        }
    }
}