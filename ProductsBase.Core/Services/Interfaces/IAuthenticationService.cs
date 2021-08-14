using System.Threading.Tasks;
using ProductsBase.Domain.Services.Communication;

namespace ProductsBase.Domain.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> CreateAccessTokenAsync(string email, string password);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken, string userEmail);
        void RevokeRefreshToken(string refreshToken);
    }
}