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
    }
}