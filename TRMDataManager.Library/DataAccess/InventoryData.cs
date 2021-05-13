using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {
            var sql = new SqlDataAccess();

            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "TRMData");

            return output; // TODO: Fails C# recommended practice, should return interface; Going along with course...
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            var sql = new SqlDataAccess();

            sql.SaveData("dbo.spInventory_Insert", item, "TRMData");
        }
    }
}