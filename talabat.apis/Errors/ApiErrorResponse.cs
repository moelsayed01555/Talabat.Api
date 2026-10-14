namespace talabat.apis.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public ApiErrorResponse( int statusCode, string? message = null )
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(statusCode);   // if message is null, get the default message based on the status code
        }

        private string? GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
