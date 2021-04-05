using System.Collections.Generic;
using TRMDataManager.Library.Internal.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string id)
        {
            var sql = new SqlDataAccess();
            var parameters = new {Id = id};
            List<UserModel> output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameters, "TRMData");
            return output; // TODO: Violated C# recommended practice, should return interface for collection. Going along with course...
        }
    }
}