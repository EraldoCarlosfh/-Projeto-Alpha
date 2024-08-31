using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Data.Connection
{
    public static class DbSetExtensions
    {
        public static IQueryable<TEntity> IncludePaths<TEntity>(this IQueryable<TEntity> source,
            params string[] navigationPaths) where TEntity : class
        {
            if (!(source.Provider is EntityQueryProvider))
            {
                return source;
            }

            return source.Include(string.Join(".", navigationPaths));
        }
    }
}
