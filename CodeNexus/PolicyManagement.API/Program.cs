using Microsoft.EntityFrameworkCore;
using PolicyManagement.API;
using PolicyManagement.API.AuthorizationMiddleWare;
using PolicyManagement.API.DIResolver;
using PolicyManagement.DAL.DBContext;
using SharedLibraries;

var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot configuration = new ConfigurationBuilder()
.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: false, reloadOnChange: true).Build();
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
builder.Services.AddDbContext<PolicyMgmtDBContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("PMSNexus")));
var appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
builder.Services.AddSingleton(appSettings!);
builder.Services.PolicyMgmtDIResolver();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var app = builder.Build();
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
//app.UseHttpsRedirection();
app.UseMiddleware<CustomAuthorizationMiddleware>().UseMiddleware<LoggingMiddleware>();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
app.MapControllers();
app.Run();