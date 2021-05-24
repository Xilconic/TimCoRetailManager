using System.Collections.Generic;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAllAsync();
        Task<Dictionary<string, string>> GetAllRolesAsync();
        Task AddUserToRoleAsync(string userId, string roleName);
        Task RemoveUserFromRoleAsync(string userId, string roleName);
    }
}