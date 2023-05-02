using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.API.AuthorizationMiddleWare
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        public readonly IConfiguration configuration;
        public CustomAuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this._next = next;
            this.configuration = configuration;
        }
        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value.StartsWith("/api/"))
            {
                if (httpContext.User.Claims.Count() > 0)
                {
                    string userEmailAddress = httpContext.User.Claims.First(c => c.Type == "preferred_username").Value;
                    string Method = httpContext.Request.Method;
                    string pathValue = httpContext.Request.Path.Value.Split("/")[2];
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
                    object result = cmd.ExecuteScalar();
                    thisConnection.Close();
                    if (result == null || result.ToString() == "0")
                    {
                        httpContext.Response.StatusCode = 401;
                        return Task.FromResult<object>(null);
                    }
                }
            }
            return _next(httpContext);
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