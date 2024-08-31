using Alpha.Domain.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Alpha.Data.Configurations.Users;
using Alpha.Framework.MediatR.Data.Converters;
using System;
using System.IO;
using Alpha.Framework.MediatR.Auditorship.Data;

namespace Alpha.Data
{
    public class AlphaDataContext : DbContext
    {
        public AlphaDataContext(DbContextOptions<AlphaDataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
      
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());          
            modelBuilder.ConfigureAuditorshipDataContext();
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }

    public class DataContextFactory : IDesignTimeDbContextFactory<AlphaDataContext>
    {
        public AlphaDataContext CreateDbContext(string[] args)
        {
            // Get environment            
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Alpha.Api"))
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AlphaDataContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            optionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedValueConverterSelector>();

            return new AlphaDataContext(optionsBuilder.Options);
        }
    }
}