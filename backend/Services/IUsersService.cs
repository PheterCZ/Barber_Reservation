using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.DTOs;

namespace backend.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
    }
}