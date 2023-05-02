using CustomerOnBoarding.API.AuthorizationMiddleWare;
using CustomerOnBoarding.API.DIResolver;
using CustomerOnBoarding.DAL.DBContext;
using CustomerOnBoarding.DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SharedLibraries;
using SharedLibraries.Authentication.Handler;
using System.Collections.Generic;

namespace CustomerOnBoarding.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var entityConnections = Configuration.GetSection(nameof(EntityConnections)).Get<EntityConnections>();
            services.AddSingleton(typeof(EntityConnections), entityConnections);
            services.AddDbContext<COBDBContext>(ServiceLifetime.Transient);
            //services.AddDbContext<SharedLibraries.DBContext.SharedDBContext>(ServiceLifetime.Transient);
            services.AddControllers();
            services.COBDIResolver();
            services.AddMemoryCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer OnBoarding API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.EnableAnnotations();
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header
                        },
                        new List<string>()
                      }
                });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddAuthentication(ConstAuthMode.DYNAMIC_AUTH)
                .AddScheme<AuthenticationSchemeOptions, AuthHandler>(ConstAuthMode.DYNAMIC_AUTH, null)
                .AddJwtBearer();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            //app.UseCors("CorsPolicy");
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            //app.UseHttpsRedirection();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "PMS Nexus");
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<CustomAuthorizationMiddleware>().UseMiddleware<LoggingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}