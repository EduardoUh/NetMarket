namespace WebApi.Errors
{
    public class CodeErrorResponse(int statusCode, string? message = null)
    {
        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message ?? GetDefaultMessageStatusCode(statusCode);

        private static string GetDefaultMessageStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "The request contains one or more errors",
                401 => "Unauthorized",
                404 => "Resource not found",
                500 => "Internal server error",
                _ => "An error occurred while performing the operation"
            };
        }

    }
}
