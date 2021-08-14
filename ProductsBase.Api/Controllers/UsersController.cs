using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsBase.Api.Resources;
using ProductsBase.Domain.Models;
using ProductsBase.Domain.Services.Interfaces;

namespace ProductsBase.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")] 
        public async Task<IEnumerable<UserResource>> GetUsersAsync()
        {
            var users = await _userService.GetUsersAsync();

            var usersResource = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);
            return usersResource;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCredentialResource userResource)
        {
            var user = _mapper.Map<UserCredentialResource, User>(userResource);

            var result = await _userService.CreateUserAsync(user, ApplicationRole.Common);

            if (!result.Success)
                return BadRequest(new ErrorResource(result.Message));

            var createdUser = _mapper.Map<User, UserResource>(result.User);

            return Ok(createdUser);
        }
    }
}