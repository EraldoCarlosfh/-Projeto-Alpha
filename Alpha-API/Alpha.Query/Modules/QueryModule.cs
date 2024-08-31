using Alpha.Domain.Queries.Users;
using Alpha.Query.Queries.Users.OctaTech.Query.Queries.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Alpha.Query.Modules
{
    public static class QueryModule
    {
        public static void ConfigureQuery(this IServiceCollection services)
        {           
            services.AddScoped<IUserQuery, UserQuery>();
            //services.AddScoped<IProductQuery, ProductQuery>();
        }
    }
}
