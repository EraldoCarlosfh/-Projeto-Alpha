using Alpha.Framework.MediatR.Data.Connection;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.EventSourcing.Modules
{
    public static class MediatorModule
    {
        public static void ConfigureMediator(this IServiceCollection services, Assembly assembly, string _databaseConnectionString)
        {
            services.AddMediatR(assembly);

            services.AddScoped<ISqlConnectionFactory>(x =>
                ActivatorUtilities.CreateInstance<SqlConnectionFactory>(x, _databaseConnectionString));

            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddSingleton<IValueConverterSelector, StronglyTypedValueConverterSelector>();
        }
    }
}
