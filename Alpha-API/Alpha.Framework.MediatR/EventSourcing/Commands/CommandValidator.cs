using FluentValidation;
using Alpha.Framework.MediatR.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;

namespace Alpha.Framework.MediatR.EventSourcing.Commands
{
    public abstract class CommandValidator<T> : AbstractValidator<T>
    {
        protected async Task<TResult> ServiceWrapper<TService, TResult>(Func<TService, Task<TResult>> func)
        {
            using (var scope = IoCResolver.Provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                var result = await func.Invoke(service);

                return result;
            }

        }

        protected async Task<TResult> ServiceWrapper<TService, TServiceSecond, TResult>(Func<TService, TServiceSecond, Task<TResult>> func)
        {
            using (var scope = IoCResolver.Provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                var serviceSecond = scope.ServiceProvider.GetRequiredService<TServiceSecond>();
                var result = await func.Invoke(service, serviceSecond);

                return result;
            }
        }

        protected async Task<TResult> ServiceWrapper<TService, TServiceSecond, TServiceThird, TResult>(Func<TService, TServiceSecond, TServiceThird, Task<TResult>> func)
        {
            using (var scope = IoCResolver.Provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                var serviceSecond = scope.ServiceProvider.GetRequiredService<TServiceSecond>();
                var serviceThird = scope.ServiceProvider.GetRequiredService<TServiceThird>();
                var result = await func.Invoke(service, serviceSecond, serviceThird);

                return result;
            }
        }

        protected TResult ServiceWrapper<TService, TResult>(Func<TService, TResult> func)
        {
            using (var scope = IoCResolver.Provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                var result = func.Invoke(service);

                return result;
            }
        }

        protected TService ServiceWrapper<TService>()
        {
            using (var scope = IoCResolver.Provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();

                return service;
            }
        }
    }
}
