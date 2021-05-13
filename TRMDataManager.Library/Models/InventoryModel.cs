using System;

namespace TRMDataManager.Library.Models
{
    public class InventoryModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}