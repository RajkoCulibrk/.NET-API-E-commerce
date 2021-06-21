using AutoMapper;
using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Users;
using ECommerce.Services.AuthServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private IMapper _mapper;
        private IAuthManager _authManager;

        public AuthController(UserManager<ApiUser> userManager, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDto userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _mapper.Map<ApiUser>(userData);

            var result = await _userManager.CreateAsync(user,userData.Password);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code,error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRoleAsync(user, "User");
            await _authManager.ValidatUuser(new LoginUserDto { Email = userData.Email, Password = userData.Password });
            APIResponse<string> response = new APIResponse<string>();
            response.Data = await _authManager.CreateToken();
            response.Message = "Successfully registrated";
            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if ( ! await _authManager.ValidatUuser(userData))
            {
                return Unauthorized();
            }
            APIResponse<string> response = new APIResponse<string>();
            response.Data = await _authManager.CreateToken();
            response.Message = "Successfull login";
            return Ok(response);
        }
     
    }
}
