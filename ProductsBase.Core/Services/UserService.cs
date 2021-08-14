using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsBase.Data.Contexts;
using ProductsBase.Domain.Models;
using ProductsBase.Domain.Security.Hashing;
using ProductsBase.Domain.Services.Communication;
using ProductsBase.Domain.Services.Interfaces;

namespace ProductsBase.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger _logger;

        public UserService(AppDbContext dbContext, IPasswordHasher passwordHasher, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _logger = loggerFactory.CreateLogger<UserService>();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _dbContext.Users.Include(u=>u.Roles).ToListAsync();
        }

        public async Task<CreateUserResponse> CreateUserAsync(User user, params ApplicationRole[] userRoles)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Email == user.Email);

            if (existingUser is not null)
                return new CreateUserResponse("User with this email already exists");

            user.Password = _passwordHasher.HashPassword(user.Password);

            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                var newUser = await _dbContext.Users.Include(u => u.Roles)
                                              .FirstOrDefaultAsync(x => x.Email == user.Email);
                return new CreateUserResponse(newUser);
            }
            catch (Exception e)
            {
                var errorMessage = $"An error occured when saving the user: {e.Message}";
                _logger.Log(LogLevel.Error, errorMessage);
                return new CreateUserResponse(errorMessage);
            }

        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.Include(u=>u.Roles).FirstOrDefaultAsync(x=>x.Email == email);
        }
    }
}