using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NMAS.WebApi.Host.Configurations;
using NMAS.WebApi.Host.Middlewares;
using System.Text.Json.Serialization;

namespace NMAS
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.ConfigureDependencyInjection(Configuration, Environment);
            services.ConfigureSwagger(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowSpecificOrigin",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000")
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                                  });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("Auth0:Authority");
                options.Audience = Configuration.GetValue<string>("Auth0:Audience");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseCors("AllowSpecificOrigin");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.ConfigureSwagger(Configuration);
        }
    }
}
