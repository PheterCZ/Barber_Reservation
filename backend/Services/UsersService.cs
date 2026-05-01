
using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.Data;
using backend.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        
        public UsersService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var dtos = _mapper.Map<IEnumerable<UserDto>>(users);

            foreach (var dto in dtos)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.ToString() == dto.Id.ToString());
                if(user != null)
                {
                    dto.Roles = await _userManager.GetRolesAsync(user);

                }
            }
            return dtos;
        }
    }
}