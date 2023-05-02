using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerOnBoarding.API.AuthorizationMiddleWare
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        public readonly IConfiguration configuration;
        public CustomAuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            this.configuration = configuration;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.StartsWith("/api/"))
            {
                if (httpContext.User.Claims.Count() > 0)
                {
                    var userEmailAddress = httpContext.User.Claims.First(c => c.Type == "preferred_username").Value;
                    var Method = httpContext.Request.Method;
                    var pathValue = httpContext.Request.Path.Value.Split("/")[2];
                    using SqlConnection thisConnection = new SqlConnection(this.configuration.GetConnectionString("PMSNexus"));
                    thisConnection.Open();
                    SqlCommand cmd = new SqlCommand("usp_CheckUserAuthorization", thisConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add("@EmailAddress", SqlDbType.VarChar).Value = userEmailAddress;
                    cmd.Parameters.Add("@Mode", SqlDbType.VarChar).Value = Method;
                    cmd.Parameters.Add("@APICall", SqlDbType.VarChar).Value = pathValue;
                    cmd.CommandTimeout = 0;
                    var result = cmd.ExecuteScalar();
                    thisConnection.Close();
                    if (result == null || result.ToString() == "0")
                    {
                        httpContext.Response.StatusCode = 401;
                        return;
                    }
                }
            }
            await _next(httpContext);
        }
    }
    public static class CustomAuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthorizationMiddleware>();
        }
    }
}