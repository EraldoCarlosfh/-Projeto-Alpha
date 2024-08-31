using Alpha.Framework.MediatR.Auditorship.Domain.Entities;
using Alpha.Framework.MediatR.Auditorship.Domain.Enums;
using Alpha.Framework.MediatR.Data.Configurations;
using Alpha.Framework.MediatR.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Auditorship.Data.Configurations
{
    public class AuditConfiguration : EntityConfigurationBase<Audit>
    {
        public override void ConfigureEntityFields(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("__Audits");
            builder.Property(c => c.OperationType).HasColumnName("OperationTypeId").HasConversion(new EnumToIntegerValueConverter<OperationType>()).IsRequired();
            builder.Property(c => c.RequestClassIdentification).IsRequired().HasMaxLength(256);
            builder.Property(c => c.RequestPayload).IsRequired();
            builder.Property(c => c.EntityClassIdentification).IsRequired().HasMaxLength(256);
            builder.Property(c => c.EntityId).IsRequired(false).HasMaxLength(128);
            builder.Property(c => c.UserIdentification).IsRequired(false).HasMaxLength(1024);
        }
    }
}
