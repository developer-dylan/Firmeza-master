using Firmeza.Web.Models.Entities;
using Firmeza.Identity;

namespace Firmeza.Web.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(string id);
        Task AddAsync(AppUser user);
        Task UpdateAsync(AppUser user);
        Task DeleteAsync(AppUser user);
        Task<bool> ExistsAsync(string id);
        Task<AppUser?> GetByNameAsync(string name);
        Task<AppUser?> GetByEmailAsync(string email);
        Task<IQueryable<AppUser>> GetQueryable();
    }
}
