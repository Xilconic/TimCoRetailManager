using System;
using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class SaleData
    {
        /// <exception cref="Exception"/>
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            // Make this SOLID/DRY/Better
            // TODO: I agree, this method is doing to many things. Can decomposed in smaller pieces.
            var products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100m; // TODO: Copied from CalculateTax.CalculateTax; Going along with course...

            // Start filling in the sale detail models we will save to the database
            var details = new List<SaleDetailDBModel>();
            foreach (var item in saleInfo.SaleDetails)
            {
                // Fill in the available information
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };

                // Get the information about this product
                var productInfo = products.GetProductById(item.ProductId);
                if (productInfo == null)
                {
                    // TODO: Violated C# recommended practice on not throwing basic Exception. Going along with course...
                    throw new Exception($"The product Id of {item.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if (productInfo.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * taxRate;
                }

                details.Add(detail);
            }

            // Create the sale model
            var sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId,
            };
            sale.Total = sale.SubTotal + sale.Tax;

            // Save the sale model
            var sql = new SqlDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "TRMData");

            // Get the ID from the sale model
            int saleId = sql.LoadData<int, dynamic>("dbo.spSale_Lookup", new { CashierId = cashierId, sale.SaleDate}, "TRMData").FirstOrDefault();

            // Finishing filling in the sale detail models.
            foreach (var item in details)
            {
                item.SaleId = saleId;

                // Save the sale detail models
                sql.SaveData("dbo.spSaleDetail_Insert", item, "TRMData");
            }
        }
    }
}