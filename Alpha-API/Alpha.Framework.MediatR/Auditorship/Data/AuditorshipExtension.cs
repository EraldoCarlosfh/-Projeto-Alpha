using Alpha.Framework.MediatR.Auditorship.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Auditorship.Data
{
    public static class AuditorshipExtension
    {
        public static void ConfigureAuditorshipDataContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuditConfiguration());
        }
    }
}
