namespace Alpha.Framework.MediatR.SecurityService.DataTransferObjects
{
    public class CreateTokenRequest
    {
        public string Id { get; set; }    
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
