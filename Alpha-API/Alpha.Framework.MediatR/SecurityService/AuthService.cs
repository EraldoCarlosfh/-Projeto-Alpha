using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.Configurations;
using Alpha.Framework.MediatR.SecurityService.DataTransferObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using OtpNet;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Alpha.Framework.MediatR.SecurityService
{
    public class AuthService : IAuthService
    {
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly SigningConfiguration _signingConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AuthService(
            TokenConfiguration tokenConfiguration,
            SigningConfiguration signingConfiguration,
            IWebHostEnvironment hostingEnvironment)
        {
            _tokenConfiguration = tokenConfiguration;
            _signingConfiguration = signingConfiguration;
            _hostingEnvironment = hostingEnvironment;
        }

        public CreateTokenResponse CreateToken(CreateTokenRequest request)
        {
            var response = new CreateTokenResponse();
            response.CreateDate = DateTime.UtcNow.ToLocalTimeZone();
            response.ExpirationDate = response.CreateDate.AddSeconds(_tokenConfiguration.Seconds);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
                new Claim(ClaimTypes.Name, request.Name),
                new Claim(ClaimTypes.Email, request.Email)
            };          


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                NotBefore = response.CreateDate,
                Expires = response.ExpirationDate
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            response.Token = tokenHandler.WriteToken(token);
            return response;
        }
        public GenerateVerificationCodeResponse GenerateVerificationCode(string secret)
        {
            if (secret == null) throw new ArgumentNullException(nameof(secret));

            if (_hostingEnvironment.EnvironmentName == "Test")
            {
                var response = new GenerateVerificationCodeResponse();
                response.Code = "999999";
                response.ExpirationDate = DateTime.UtcNow.ToLocalTimeZone().AddHours(_tokenConfiguration.VerificationCodeExpirationInHours);

                return response;
            }
            if (secret == "")
            {
                var response = new GenerateVerificationCodeResponse();
                response.Code = "";
                response.ExpirationDate = DateTime.UtcNow.ToLocalTimeZone();

                return response;
            }
            else
            {
                var topt = new Totp(
                    Encoding.Default.GetBytes(secret),
                    step: _tokenConfiguration.VerificationCodeStep,
                    totpSize: _tokenConfiguration.VerificationCodeTopSize);

                var response = new GenerateVerificationCodeResponse();
                response.Code = topt.ComputeTotp();
                response.ExpirationDate = DateTime.UtcNow.ToLocalTimeZone().AddHours(_tokenConfiguration.VerificationCodeExpirationInHours);

                return response;
            }
        }

    }
}

