using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Alpha.Data.Configurations.Users
{
    public class ProductConfiguration : EntityConfigurationBase<Product>
    {
        public override void ConfigureEntityFields(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.Property(c => c.Name).HasMaxLength(128).IsRequired();
            builder.Property(c => c.BarCode).HasMaxLength(128).IsRequired();
            builder.Property(c => c.Image).IsRequired(false).HasMaxLength(1024); 
            builder.Property(c => c.Price).IsRequired(false);
        }
    }
}