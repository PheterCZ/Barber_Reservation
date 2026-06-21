using backend.Data;
using backend.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDBContext _context;

        public UsersService(ApplicationDBContext context)
        {
            _context = context;
        }
        

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            var userRoles = await (
                from ur in _context.UserRoles
                join r in _context.Roles
                    on ur.RoleId equals r.Id
                select new
                {
                    ur.UserId,
                    RoleName = r.Name
                }
            ).ToListAsync();

            var rolesLookup = userRoles
                .Where(x => !string.IsNullOrWhiteSpace(x.RoleName))
                .GroupBy(x => x.UserId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.RoleName!).ToList()
                );

            return users.Select(u => new UserDto(
                u.Id,
                u.FullName,
                u.Email ?? string.Empty,
                rolesLookup.GetValueOrDefault(u.Id) ?? new List<string>()
            ));
        }
    }
}