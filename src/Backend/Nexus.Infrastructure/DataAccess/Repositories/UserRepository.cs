using Microsoft.EntityFrameworkCore;
using Nexus.Domain.Entities;
using Nexus.Domain.Repositories;


namespace Nexus.Infrastructure.DataAccess.Repositories;

public class UserRepository : IRepository<User,string>
{
    private readonly NexusDbContext _context;
    public UserRepository(NexusDbContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
    public Task UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    

    
}
