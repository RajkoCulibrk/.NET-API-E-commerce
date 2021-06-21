using ECommerce.Dtos;
using ECommerce.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.UserServices
{
    public interface IUserService
    {
        Task<APIResponse<GetUserDto>> GetUser(string userId);
        Task<APIResponse<GetUserDto>> UpdateUser(string userId,UpdateUserDto userData);

    }
}
