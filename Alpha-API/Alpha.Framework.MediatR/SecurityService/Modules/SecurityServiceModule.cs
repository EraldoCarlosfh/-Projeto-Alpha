using Alpha.Framework.MediatR.SecurityService.Configurations;
using Alpha.Framework.MediatR.SecurityService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Alpha.Framework.MediatR.SecurityService.Modules
{
    public static class SecurityServiceModule
    {
        public static void ConfigureSecurityService(this IServiceCollection services, TokenConfiguration tokenConfiguration)
        {
            services.AddSingleton(tokenConfiguration);
            services.AddSingleton<IAuthService, AuthService>();

            var signing =
                new SigningConfiguration(tokenConfiguration.Secret);

            services.AddSingleton(signing);

            services
                .AddAuthorization(auth =>
                {
                    auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build()); 
                });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters =
                    new TokenValidationParameters()
                    {
                        IssuerSigningKey = signing.Key,
                        ValidAudience = tokenConfiguration.Audience,
                        ValidIssuer = tokenConfiguration.Issuer,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(0)
                    };

                x.Validate();               
            });

            services.AddHttpContextAccessor();

            services.AddTransient(s =>
            {
                var context = s.GetService<IHttpContextAccessor>();
                var result = new AuthenticatedUserModel();

                if (context?.HttpContext != null)
                {
                    var headers = context.HttpContext.Request.Headers;
                    var authKey = headers.Keys.FirstOrDefault(k => k.ToLowerInvariant() == "authorization");

                    if (authKey != null)
                    {
                        var tokenContent = headers[authKey].ToString().Replace("bearer ", "", StringComparison.InvariantCultureIgnoreCase);
                        var token = new JwtSecurityToken(tokenContent);

                        if (token != null)
                        {
                            var claims = token.Claims;

                            result.Id = claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                            result.FullName = claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                            result.Email = claims.FirstOrDefault(c => c.Type == "email")?.Value; 
                        }
                    }
                }

                return result;
            });
        }
    }
}
