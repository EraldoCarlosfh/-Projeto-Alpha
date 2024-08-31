using System;

namespace Alpha.Framework.MediatR.SecurityService.DataTransferObjects
{
    public class CreateTokenResponse
    {
        public string Token { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
