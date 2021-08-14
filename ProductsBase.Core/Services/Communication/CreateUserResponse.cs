using System;
using ProductsBase.Domain.Models;

namespace ProductsBase.Domain.Services.Communication
{
    public class CreateUserResponse : BaseResponse
    {
        public User User { get; private set; }

        public CreateUserResponse(bool success, string message, User user)
            : base(success, message)
        {
            User = user;
        }

        public CreateUserResponse(User user) : this(true,String.Empty, user)
        { }

        public CreateUserResponse(string message) : this(false, message, null)
        { }
    }
}