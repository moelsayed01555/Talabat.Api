namespace talabat.apis.Errors
{
    public class ApiValidationErrorResponse : ApiErrorResponse
    {
        // Ever Validation Error has status code 400 , message , array of errors
        // ( status code and message are inherited from ApiErrorResponse class)

        public IEnumerable<string> Errors { get; set; } 



        public ApiValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }

        
    }
}
