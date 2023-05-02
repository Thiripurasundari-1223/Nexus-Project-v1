using APIGateWay.API.Common;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibraries.Common;
using System;
using System.Collections.Generic;

namespace APIGateWay.API
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
            services.AddControllers(config =>
            {
                config.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
            });
            services.AddControllers();            
            Action<IdentityServerAuthenticationOptions> opt = o =>
            {
                o.Authority = "http://localhost:1080";
                o.ApiName = "SampleService";
                o.SupportedTokens = SupportedTokens.Both;
                o.RequireHttpsMetadata = false;
            };
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                //c.EnableAnnotations();
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            //services.AddAuthentication(ConstAuthMode.DYNAMIC_AUTH)
            //    .AddScheme<AuthenticationSchemeOptions, AuthHandler>(ConstAuthMode.DYNAMIC_AUTH, null)
            //    .AddJwtBearer();
            services.AddSingleton<LoggerManager, LoggerManager>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer("NexusAPI", options =>
               {

                   options.Authority = "http://localhost:5000"; // auth server base endpoint (will use to search for disco doc)
                   options.Audience = "NexusAPI"; // required audience of access tokens
                   options.RequireHttpsMetadata = false; // dev only!
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       LifetimeValidator = (notBefore, expires, securityToken, validationParameter) =>
                         expires >= DateTime.UtcNow
                   };
               });
            //.AddIdentityServerAuthentication("NexusAPI", options => {
            //    options.Authority = "http://localhost:5000";
            //    options.ApiName = "NexusAPI";
            //    options.RequireHttpsMetadata = false;
            //    options.ApiSecret = "Nexus@Tvsn2021";
            //    options.EnableCaching = false;

            //});


        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
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

            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                    endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
                else
                    endpoints.MapControllers();
            });
        }
    }
}