using AutoMapper;
using ECommerce.Data;
using ECommerce.Data.Models;
using ECommerce.Dtos;
using ECommerce.Dtos.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public UserService(DataContext context, IMapper mapper, UserManager<ApiUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<APIResponse<GetUserDto>> GetUser(string userId)
        {
            APIResponse<GetUserDto> response = new APIResponse<GetUserDto>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
           
            
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode=404;
            }
            else
            {
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");
                GetUserDto getUserDto= _mapper.Map<GetUserDto>(user);
                getUserDto.IsAdmin = isAdmin;
                response.Data = getUserDto;
                response.Success = true;
                response.StatusCode=200;
            }

            

            return response;
        }

        public async Task<APIResponse<GetUserDto>> UpdateUser(string userId, UpdateUserDto userData)
        {
            APIResponse<GetUserDto> response = new APIResponse<GetUserDto>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            _mapper.Map(userData, user);
             _context.Users.Update(user);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetUserDto>(user);
            response.Success = true;
            response.StatusCode = 200;
            response.Message = "Accound data updated successfully.";

            return response;
        }
    }
}
