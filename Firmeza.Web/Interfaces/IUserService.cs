using Firmeza.Identity;
using Firmeza.Web.Models;

namespace Firmeza.Web.Interfaces
{
    public interface IUserService
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(string id);
        Task CreateAsync(AppUser user);
        Task UpdateAsync(AppUser user);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task CreateWithPasswordAsync(AppUser user, string password, string role);
        Task<PaginatedList<AppUser>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
