namespace Alpha.Api.ViewModels.Users
{
    public class LoginResponseViewModel : IViewModel
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public UserViewModel User { get; set; }
        public int PasswordErrorsAllowed { get; set; }
        public string UserId { get; set; }
        public bool? IsAccessActivated { get; set; }
        public bool? IsApproved { get; set; }
        public bool UserExists { get; set; }
        public bool? IsRegisteredDetran { get; set; }
        public bool IsApprovedNotificationViewed { get; set; }
        public bool IsNewNotification { get; set; }
        public bool? IsExpirationDate { get; set; }
    }
}
