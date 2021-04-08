using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public class ProductEndpoint : IProductEndpoint
    {
        private readonly IApiHelper _apiHelper;

        public ProductEndpoint(
            IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        /// <exception cref="Exception"/>
        public async Task<List<ProductModel>> GetAllAsync() // TODO: Returning concrete collection types is violating C# guidelines; Just following along with the course...
        {
            // TODO: Similar code pattern with ApiHelper.AuthenticateAsync. Just following along with the course...
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Product"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
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