using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            var data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userId);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            // TODO: Fails C# recommended practice, should return interface; Going along with course...
            // TODO: This has direct coupling between API callers and database schema, would rather isolate this; Going along with course...
            var data = new SaleData();
            return data.GetSaleReport();
        }
    }
}
