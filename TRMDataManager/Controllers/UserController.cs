using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public UserModel GetById()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData(); // TODO: Creates hard database dependency, limiting unit testing possibilities. Going along with course...

            // TODO: I'm not a fan of directly returning the Database Model/Entity object across an API,
            //       as that creates coupling between the database schema and the consumers of this endpoint.
            //       I'd rather introduce an adapter/DTO object to isolate that dependency.
            //       But I'll just go along with the course...
            return data.GetUserById(userId).First(); // TODO: Returning a single user, should be responsibility of the `GetUserById` method called. Going along with course...
        }
    }
}
