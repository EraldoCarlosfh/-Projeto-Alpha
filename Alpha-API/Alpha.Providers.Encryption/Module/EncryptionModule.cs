using Alpha.Integrations.Encryption;
using Alpha.Providers.Encryption.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Providers.Encryption.Module
{
    public static class EncryptionModule
    {
        public static void ConfigureEncryption(this IServiceCollection services, CryptoConfiguration cryptoConfiguration)
        {
            services.AddSingleton<IEncryptionService>(x =>
                ActivatorUtilities.CreateInstance<EncryptionService>(x, cryptoConfiguration));
        }
    }
}
