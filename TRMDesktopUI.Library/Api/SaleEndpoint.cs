using System;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public SaleEndpoint(
            IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostSaleAsync(SaleModel sale)
        {
            // TODO: Similar code pattern with ProductEndpoint. Just following along with the course...
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    // Log successful call?
                }
                else
                {
                    // TODO: Not C# recommended practice, but just following along with the course.
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}