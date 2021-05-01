using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            var sql = new SqlDataAccess();
            var parameters = new { };
            List<ProductModel> output = sql.LoadData<ProductModel, dynamic>("dbo.sp_Product_GetAll", parameters, "TRMData");
            return output; // TODO: Violated C# recommended practice, should return interface for collection. Going along with course...
        }

        public ProductModel GetProductById(int productId)
        {
            var sql = new SqlDataAccess();
            var parameters = new { Id = productId };
            ProductModel output = sql.LoadData<ProductModel, dynamic>("dbo.sp_Product_GetById", parameters, "TRMData").FirstOrDefault();
            return output;
        }
    }
}