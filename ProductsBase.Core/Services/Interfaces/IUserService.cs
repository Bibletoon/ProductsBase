using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsBase.Domain.Models;
using ProductsBase.Domain.Services.Communication;

namespace ProductsBase.Domain.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<CreateUserResponse> CreateUserAsync(User user, params ApplicationRole[] userRoles);
        Task<User> FindByEmailAsync(string email);
    }
}