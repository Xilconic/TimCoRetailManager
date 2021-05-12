using System;

namespace TRMDesktopUI.Library.Models
{
    public class LoggedInUserModel : ILoggedInUserModel
    {
        public string Token { get; set; }
        public string AuthUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }

        public void ResetUserModel()
        {
            Token = string.Empty;
            AuthUserId = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            EmailAddress = String.Empty;
            CreatedDate = DateTime.MinValue;
        }
    }
}