namespace Alpha.Api.ViewModels.Users
{
    public class LoginPhoneGenerateCodeResponseViewModel : IViewModel
    {
        public string LoginPhoneCode { get; set; }
        public string UserId { get; set; }
        public string PersonId { get; set; }
        public bool? IsAccessActivated { get; set; }
        public bool IsApproved { get; set; }
        public bool UserExists { get; set; }
    }
}
