using System;

namespace Alpha.Framework.MediatR.SecurityService.DataTransferObjects
{
    public class GenerateVerificationCodeResponse
    {
        public string Code { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
