using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Timesheet.API.DIResolver;
using Timesheet.DAL.DBContext;
using Timesheet.DAL.Models;

namespace Timesheet.API
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
            services.AddDbContext<TSDBContext>(ServiceLifetime.Transient);
            services.AddControllers();
            services.TSDIResolver();
            services.AddMemoryCache();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddMvc().AddNewtonsoftJson();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Timesheet API", Version = "v1" });
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
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                });                
            });
            //var appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            //services.AddSingleton(appSettings);
            //services.AddAuthentication(ConstAuthMode.DYNAMIC_AUTH)
            //   .AddScheme<AuthenticationSchemeOptions, AuthHandler>(ConstAuthMode.DYNAMIC_AUTH, null)
            //   .AddJwtBearer();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            //app.UseMiddleware<CustomAuthorizationMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();
            app.UseRouting();
            //app.UseCors("CorsPolicy");
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "PMS Nexus");
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}