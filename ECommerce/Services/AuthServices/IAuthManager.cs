using ECommerce.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.AuthServices
{
    public interface IAuthManager
    {
        Task<bool> ValidatUuser(LoginUserDto userDto);
        Task<string> CreateToken();
    }
}
