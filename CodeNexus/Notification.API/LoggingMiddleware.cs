using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Employee;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Notification.API
{
    public class LoggingMiddleware
    {

        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }
                Globals.Id = DateTime.Now.ToString("ddMMyyyyhhmmss");
                context.Request.EnableBuffering();
                var builder = new StringBuilder();
                var request = await FormatRequest(context.Request);
                builder.Append("Id: ").Append(Globals.Id).Append("\t");
                builder.Append("RequestDateAndTime: ").Append(DateTime.Now.ToString()).Append("\t");
                builder.Append("Request: ").AppendLine(request).Append("\t");
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);
                    var response = await FormatResponse(context.Response);
                    builder.Append("ResponseDateAndTime: ").Append(DateTime.Now.ToString()).Append("\t");
                    builder.Append("Response: ").AppendLine(response).Append("\t");
                    await responseBody.CopyToAsync(originalBodyStream);
                    LoggerManager.LoggingMessage(builder.ToString());
                }
            }
            catch (IOException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(context), string.Empty, string.Empty);
            }
            catch (SqlException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(context), string.Empty, string.Empty);
            }
            catch (Exception Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(context), string.Empty, string.Empty);
            }
        }
        private async Task<string> FormatRequest(HttpRequest request)
        {
            try
            {
                // Leave the body open so the next middleware can read it.
                using (var reader = new StreamReader(
                    request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    // Do some processing with body…
                    var formattedRequest = $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {body}";
                    // Reset the request body stream position so the next middleware can read it
                    request.Body.Position = 0;
                    return formattedRequest;
                }
            }
            catch (IOException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(request), string.Empty, string.Empty);
                return null;
            }
            catch (SqlException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(request), string.Empty, string.Empty);
                return null;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(request), string.Empty, string.Empty);
                return null;
            }
            
        }
        private async Task<string> FormatResponse(HttpResponse response)
        {
            try
            {
                //We need to read the response stream from the beginning...
                response.Body.Seek(0, SeekOrigin.Begin);
                //...and copy it into a string
                using (var reader = new StreamReader(
                    response.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true))
                {
                    string text = await reader.ReadToEndAsync();
                    //We need to reset the reader for the response so that the client can read it.
                    response.Body.Seek(0, SeekOrigin.Begin);
                    //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
                    return $"{response.StatusCode}: {text}";
                }
            }
            catch (IOException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(response), string.Empty, string.Empty);
                return null;
            }
            catch (SqlException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(response), string.Empty, string.Empty);
                return null;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, MethodBase.GetCurrentMethod().Name, string.Empty, JsonConvert.SerializeObject(response), string.Empty, string.Empty);
                return null;
            }
        }
    }
}
