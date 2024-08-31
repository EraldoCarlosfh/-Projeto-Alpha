using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.IoC
{
    public class IoCResolver
    {
        public static IServiceProvider Provider { get; private set; }
        public static void Inicialize(IServiceProvider provider) => Provider = provider;

        public async Task<TResult> ServiceWrapper<TService, TResult>(Func<TService, Task<TResult>> func)
        {
            using (var scope = Provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                var result = await func.Invoke(service);

                return result;
            }
        }

        public TResult ServiceWrapper<TService, TResult>(Func<TService, TResult> func)
        {
            using (var scope = Provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                var result = func.Invoke(service);

                return result;
            }
        }
    }
}
