using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        /// <exception cref="Exception"/>
        Task<List<ProductModel>> GetAllAsync(); // TODO: Returning concrete collection types is violating C# guidelines; Just following along with the course...
    }
}