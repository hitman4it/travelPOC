using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Web.Http;

namespace OpenTelemetryTests.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestMiddleware> logger;

        public RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ErrorResponse() { Errors = new List<string>() { error.Message } };
                switch (error)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case HttpResponseException e:
                        response.StatusCode = (int)e.Response.StatusCode;
                        var responseMessage = await e.Response.Content.ReadAsStringAsync();
                        responseModel.Errors = new List<string>() { responseMessage };
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Errors = new List<string>() { "Unexpected error occured while executing the request" };
                        break;
                }

                logger.LogError($"Error occured with request {context.Request?.Method} {context.Request?.Path.Value} => {context.Response?.StatusCode} | Exception: {error}");
                responseModel.Status = response.StatusCode;
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                };
                var result = JsonSerializer.Serialize(responseModel, serializeOptions);
                await response.WriteAsync(result);
            }
            finally
            {
                logger.LogInformation("Request {RequestMethod} {RequestPath} => {ResponseStatusCode}  | Total time taken :{APIResponseTime}ms", context.Request?.Method, context.Request?.Path.Value, context.Response?.StatusCode, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
