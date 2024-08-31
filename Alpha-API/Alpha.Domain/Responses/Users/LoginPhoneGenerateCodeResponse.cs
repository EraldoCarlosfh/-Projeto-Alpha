namespace Alpha.Domain.Responses.Users
{
    public class LoginPhoneGenerateCodeResponse
    {
        public LoginPhoneGenerateCodeResponse()
        {
            SentSmsCode = false;
        }

        public string UserId { get; set; }
        public string PersonId { get; set; }
        public bool? IsAccessActivated { get; set; }
        public bool IsApproved { get; set; }
        public bool UserExists { get; set; }
        public bool SentSmsCode
        {
            get; set;
        }
    }
}
