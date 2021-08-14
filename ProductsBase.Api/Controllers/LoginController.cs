using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsBase.Api.Resources;
using ProductsBase.Domain.Security.Tokens;
using ProductsBase.Domain.Services.Interfaces;

namespace ProductsBase.Api.Controllers
{
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IMapper mapper, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
        }
        
        [Route("/api/login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] UserCredentialResource credentialResource)
        {
            var response =
                await _authenticationService.CreateAccessTokenAsync(credentialResource.Email,
                                                                    credentialResource.Password);

            if (!response.Success)
                return BadRequest(new ErrorResource(response.Message));

            var tokenResource = _mapper.Map<AccessToken, AccessTokenResource>(response.Token);

            return Ok(tokenResource);
        }
        
        [Route("/api/token/refresh")]
        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenResource refreshTokenResource)
        {
            var response = await _authenticationService.RefreshTokenAsync(refreshTokenResource.Token, refreshTokenResource.UserEmail);
            if(!response.Success)
            {
                return BadRequest(response.Message);
            }
           
            var tokenResource = _mapper.Map<AccessToken, AccessTokenResource>(response.Token);
            return Ok(tokenResource);
        }

        [Route("/api/token/revoke")]
        [HttpPost]
        public IActionResult RevokeToken([FromBody] RevokeTokenResource revokeTokenResource)
        {
            _authenticationService.RevokeRefreshToken(revokeTokenResource.Token);
            return NoContent();
        }
    }
}