using System;

namespace ProductsBase.Domain.Security.Tokens
{
    public class JsonWebToken
    {
        public string Token { get; protected set; }
        public long Expiration { get; protected set; }

        public JsonWebToken(string token, long expiration)
        {
            Token = token;
            Expiration = expiration;
        }

        public bool IsExpired => DateTime.UtcNow.Ticks > Expiration;
    }
}