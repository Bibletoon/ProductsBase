namespace ProductsBase.Domain.Security.Tokens
{
    public class AccessToken : JsonWebToken
    {
        public RefreshToken RefreshToken { get; private set; }

        public AccessToken(string token, long expiration, RefreshToken refreshToken)
            : base(token, expiration)
        {
            RefreshToken = refreshToken;
        }
        
        
    }
}