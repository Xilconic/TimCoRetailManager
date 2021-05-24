using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public UserEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<UserModel>> GetAllAsync() // TODO: C# collection guidelines -> use interface; Going along with course...
        {
            // TODO: DRY - repeated pattern with SalesEndpoint; Going along with course...
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/user/Admin/GetAllUsers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UserModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase); // TODO: Throwing plain Exception not recommended practice; Going along with course...
                }
            }
        }

        public async Task<Dictionary<string, string>> GetAllRolesAsync() // TODO: C# collection guidelines -> use interface; Going along with course...
        {
            // TODO: DRY - repeated pattern with SalesEndpoint; Going along with course...
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("api/user/Admin/GetAllRoles"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase); // TODO: Throwing plain Exception not recommended practice; Going along with course...
                }
            }
        }

        public async Task AddUserToRoleAsync(string userId, string roleName)
        {
            // TODO: DRY - repeated pattern with SalesEndpoint; Going along with course...
            var data = new {userId, roleName};
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/user/Admin/AddRole", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase); // TODO: Throwing plain Exception not recommended practice; Going along with course...
                }
            }
        }

        public async Task RemoveUserFromRoleAsync(string userId, string roleName) // TODO: DRY - duplicate pattern from AddUserToRoleAsync; Going along with course...
        {
            // TODO: DRY - repeated pattern with SalesEndpoint; Going along with course...
            var data = new { userId, roleName };
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/user/Admin/RemoveRole", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase); // TODO: Throwing plain Exception not recommended practice; Going along with course...
                }
            }
        }
    }
}