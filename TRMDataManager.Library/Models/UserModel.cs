using System;

namespace TRMDataManager.Library.Models
{
    public class UserModel
    {
        public string AuthUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}