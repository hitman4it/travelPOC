
using Microsoft.AspNetCore.Mvc;

namespace OpenTelemetryTests.Middleware
{
    public class ErrorResponse
    {
        public int Status { get; set; }

        public IList<string> Errors { get; set; }

        public static ObjectResult BadRequest(string errorMessage)
        {
            return GetErrorResponse(StatusCodes.Status400BadRequest, errorMessage);
        }

        public static ObjectResult BadRequest(IList<string> errorMessages)
        {
            return GetErrorResponse(StatusCodes.Status400BadRequest, errorMessages);
        }

        public static ObjectResult InternalServerError(string errorMessage)
        {
            return GetErrorResponse(StatusCodes.Status500InternalServerError, errorMessage);
        }

        public static ObjectResult Conflict(string errorMessage)
        {
            return GetErrorResponse(StatusCodes.Status409Conflict, errorMessage);
        }

        public static ObjectResult NotFound(string errorMessage)
        {
            return GetErrorResponse(StatusCodes.Status404NotFound, errorMessage);
        }

        public static ObjectResult Forbidden(string errorMessage)
        {
            return GetErrorResponse(StatusCodes.Status403Forbidden, errorMessage);
        }

        public static ObjectResult Unauthorized(string errorMessage)
        {
            return GetErrorResponse(StatusCodes.Status401Unauthorized, errorMessage);
        }

        public static ObjectResult NoContent(string errorMessage)
        {
            return GetErrorResponse(StatusCodes.Status204NoContent, errorMessage);
        }

        public static ObjectResult MultiStatusError(string errorMessage)
        {
            return GetErrorResponse(StatusCodes.Status207MultiStatus, errorMessage);
        }

        private static ObjectResult GetErrorResponse(int status, string errorMessage)
        {
            return GetErrorResponse(status, new List<string> { errorMessage });
        }

        private static ObjectResult GetErrorResponse(int status, IList<string> errorMessages)
        {
            var errorResponse = new ErrorResponse
            {
                Status = status,
                Errors = errorMessages,
            };
            return new ObjectResult(errorResponse)
            {
                StatusCode = errorResponse.Status,
            };
        }
    }
}