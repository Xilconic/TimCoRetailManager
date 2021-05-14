using System.Collections.Generic;
using System.Linq;

namespace TRMDesktopUI.Library.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();

        public string RoleList // TODO: Should introduce DisplayModel for this class, with this property there; Going along with course...
        {
            get
            {
                return string.Join(", ", Roles.Select(x => x.Value));
            }
        }
    }
}