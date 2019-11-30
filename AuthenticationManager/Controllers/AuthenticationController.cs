using AuthenticationManager.Business.Interfaces;
using AuthenticationManager.Models;
using AuthenticationManager.Persisted.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationManager.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(
            IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody]SignInModel signInModel)
        {
            var authUser = await _userService.SignInAsync(signInModel.Username, signInModel.Password);

            if (authUser == null)
            {
                return Unauthorized();
            }

            return Ok(authUser);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody]SignUpModel model)
        {
            AuthUser authUser = await _userService.SignUpAsync(model.Username, model.Email, model.Password);

            if (authUser == null)
            {
                return Unauthorized();
            }

            return Ok(authUser);
        }

        [HttpPost]
        [Route("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await _userService.SignOutAsync();

            return Ok();
        }

        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPut]
        [Route("GiveAdminRole")]
        public async Task<IActionResult> GiveAdminRole([FromQuery]string userId)
        {
            var identityResult = await _userService.GiveAdminRole(userId);

            return Ok(identityResult);
        }

        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromQuery]string name)
        {
            var identityResult = await _userService.CreateRole(name);

            return Ok(identityResult);
        }
    }

}