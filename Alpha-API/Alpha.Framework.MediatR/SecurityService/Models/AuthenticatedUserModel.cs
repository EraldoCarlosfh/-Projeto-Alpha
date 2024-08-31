namespace Alpha.Framework.MediatR.SecurityService.Models
{
    public class AuthenticatedUserModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
