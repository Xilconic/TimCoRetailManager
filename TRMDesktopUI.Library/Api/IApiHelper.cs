using System.Net.Http;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public interface IApiHelper
    {
        // TODO: This feels like overexposing data that should be encapsulated; Going along with course...
        HttpClient ApiClient { get; }

        // TODO: Should be expected that the authentication fails for incorrect username/password combo
        //       This shouldn't result in an Exception being thrown. Just following along with the course though.
        /// <exception cref="System.Exception">Thrown when the authentication failed
        /// or the combination of username/password is incorrect.</exception>
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);

        /// <exception cref="System.Exception">Thrown when request failed.</exception>
        Task GetLoggedInUserInfoAsync(string token);
    }
}