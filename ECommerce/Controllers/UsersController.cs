using ECommerce.Dtos.Users;
using ECommerce.Helpers;
using ECommerce.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserData()
        {
            var userId = User.GetUserId();
            var response = await _userService.GetUser(userId);
            if (response.StatusCode == 404)
            {
                return NotFound(response);
            }
            return Ok   (response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccountInfo( [FromBody] UpdateUserDto data)
        {
            var userId = User.GetUserId();
            var response = await _userService.UpdateUser(userId,data);
            return Ok(response);
        }
    }
}
