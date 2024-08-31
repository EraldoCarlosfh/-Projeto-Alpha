using Alpha.Api.Mappings.Users;
using Alpha.Domain.Configurations;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Integrations.Encryption;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Alpha.Api.Mappings
{
    public static class AutoMapperConfiguration
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                var encryptionService = provider.GetService<IEncryptionService>();
                var authenticatedUser = provider.GetService<AuthenticatedUserModel>();
                var contextSettings = provider.GetService<ContextSettingsConfiguration>();
              
                cfg.AddProfile(new UserMappingProfile(encryptionService));
            }).CreateMapper());
        }
    }
}