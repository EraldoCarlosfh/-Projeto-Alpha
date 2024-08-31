using Alpha.Domain.Entities;
using System;

namespace Alpha.Domain.Responses.Users
{
    public class LoginUserResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public User User { get; set; }
        public int PasswordErrorsAllowed { get; set; }
        public string UserId { get; set; }
        public string PersonId { get; set; }
        public bool UserExists { get; set; }
        public bool? IsAccessActivated { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsApprovedNotificationViewed { get; set; }
        public bool IsNewNotification { get; set; }
        public bool? InvalidLoginCode { get; set; }
        public bool? IsExpirationDate { get; set; }

        public LoginUserResponse()
        {
            UserExists = true;
        }
    }
}
