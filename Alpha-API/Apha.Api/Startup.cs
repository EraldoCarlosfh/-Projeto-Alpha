using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;
using Alpha.Framework.MediatR.IoC;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Alpha.Framework.MediatR.EventSourcing.Modules;
using Alpha.Framework.MediatR.SecurityService.Modules;
using Alpha.Framework.MediatR.SecurityService.Configurations;
using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Alpha.Data.Modules;
using Alpha.Query.Modules;
using Alpha.Domain.Configurations;
using Alpha.Api.Mappings;

namespace Alpha.Api
{
    public class Startup
    {
        private readonly Assembly _assembly;
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _assembly = typeof(User).GetTypeInfo().Assembly;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<Data.AlphaDataContext>(options =>
                {
                    options
                        .UseSqlServer(Configuration.GetConnectionString("SQLConnection"))
                        .ReplaceService<IValueConverterSelector, StronglyTypedValueConverterSelector>();


                }, ServiceLifetime.Scoped);

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            var contextSettings = Configuration.GetSection("ContextSettings").Get<ContextSettingsConfiguration>();
            services.AddSingleton(contextSettings);       

            services.ConfigureMediator(_assembly, Configuration.GetConnectionString("SQLConnection"));
            services.ConfigureSecurityService(Configuration.GetSection("Token").Get<TokenConfiguration>());

            services.ConfigureAutoMapper();

            services.ConfigureData();
            services.ConfigureQuery();

            var envName = Configuration.GetSection("EnvironmentName").Value;
            var versionName = Configuration.GetSection("VersionName").Value;

            services.AddSwaggerGen(swagger =>
            {
                swagger.CustomSchemaIds(type => type.ToString());

                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = versionName,
                    Title = $"Octa Tech API - {envName}",
                    Description = "Servicos da Plataforma Octa Tech",
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });


            services.AddHealthChecks();

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddOptions();
            services.AddHealthChecks();
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/html";

                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                        {
                            await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
                        }

                        await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512));
                    });
                });
                app.UseHsts();

                // Enable Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Aplha.Api"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });   

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");

            });

            IoCResolver.Inicialize(app.ApplicationServices);
        }
    }
}