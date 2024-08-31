using Alpha.Framework.MediatR.Data.Repositories;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Alpha.Data.Modules
{
    public static class DataModule
    {
        public static void ConfigureData(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(AlphaRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
