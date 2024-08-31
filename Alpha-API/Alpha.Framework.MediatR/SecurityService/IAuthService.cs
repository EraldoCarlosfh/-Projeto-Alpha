using Alpha.Framework.MediatR.SecurityService.DataTransferObjects;

namespace Alpha.Framework.MediatR.SecurityService
{
    public interface IAuthService
    {
        CreateTokenResponse CreateToken(CreateTokenRequest request);
        GenerateVerificationCodeResponse GenerateVerificationCode(string secret);
    }
}