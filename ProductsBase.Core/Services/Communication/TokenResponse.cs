using System;
using ProductsBase.Domain.Security.Tokens;

namespace ProductsBase.Domain.Services.Communication
{
    public class TokenResponse : BaseResponse
    {
        public AccessToken Token;
        
        public TokenResponse(bool success, string message, AccessToken token)
            : base(success, message)
        {
            Token = token;
        }

        public TokenResponse(AccessToken token) : this(true,String.Empty,token)
        { }

        public TokenResponse(string Message) : this(false, Message, null)
        { }
    }
}