using Alpha.Framework.MediatR.Data.Connection;
using Alpha.Framework.MediatR.Data.Converters;
using Alpha.Framework.MediatR.EventSourcing.Domains;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Alpha.Framework.MediatR.EventSourcing.Modules
{
    public static class MediatorModule
    {
        public static void ConfigureMediator(this IServiceCollection services, Assembly assembly, string _databaseConnectionString)
        {
            services.AddMediatR(assembly);

            services.AddScoped<ISqlConnectionFactory>(x =>
                ActivatorUtilities.CreateInstance<Data.Connection.SqlConnectionFactory>(x, _databaseConnectionString));

            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddSingleton<IValueConverterSelector, StronglyTypedValueConverterSelector>();
        }
    }
}
