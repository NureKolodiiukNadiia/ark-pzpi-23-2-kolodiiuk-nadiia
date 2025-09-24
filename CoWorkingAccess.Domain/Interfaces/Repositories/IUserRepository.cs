using CoWorkingAccess.Domain.Common;
using CoWorkingAccess.Domain.Entities;

namespace CoWorkingAccess.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<Result<User>> GetUserByEmailPassword(string email, string password);
}
