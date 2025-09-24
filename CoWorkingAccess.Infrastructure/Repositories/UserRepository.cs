using CoWorkingAccess.Domain.Common;
using CoWorkingAccess.Domain.Entities;
using CoWorkingAccess.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoWorkingAccess.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(CoWorkingAccessDbContext context) : base(context)
    {
    }

    public async Task<Result<User>> GetUserByEmailPassword(string email, string password)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

            return Result.Success(user);
        }
        catch (Exception e)
        {
            return Result.Fail<User>($"Error getting user {email}: {e.Message}");
        }
    }

    public Task<Result<User>> CreateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
