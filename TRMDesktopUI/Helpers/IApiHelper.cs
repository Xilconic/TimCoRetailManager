using System.Threading.Tasks;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.Helpers
{
    public interface IApiHelper
    {
        // TODO: Should be expected that the authentication fails for incorrect username/password combo
        //       This shouldn't result in an Exception being thrown. Just following along with the course though.
        /// <exception cref="System.Exception">Thrown when the authentication failed
        /// or the combination of username/password is incorrect.</exception>
        Task<AuthenticatedUser> AuthenticateAsync(string username, string password);
    }
}