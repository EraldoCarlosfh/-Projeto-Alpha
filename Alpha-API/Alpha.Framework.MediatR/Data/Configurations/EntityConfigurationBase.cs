using Alpha.Framework.MediatR.EventSourcing.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Framework.MediatR.Data.Configurations
{
    public abstract class EntityConfigurationBase<T> : IEntityTypeConfiguration<T> where T : Entity<T>
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.IsActive).HasDefaultValue(true);
            builder.Property(c => c.CreatedAt).IsRequired();

            ConfigureEntityFields(builder);

            Seed(builder);
        }

        public abstract void ConfigureEntityFields(EntityTypeBuilder<T> builder);

        public virtual void Seed(EntityTypeBuilder<T> builder)
        {
            // Do nothing. Only for concrete class implementations.
        }
    }
}
