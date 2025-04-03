namespace TerracoDaCida.Configuration
{
    public class CustomResponse
    {
        public CustomResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            errorMessage = message;
        }

        public int StatusCode { get; set; }
        public string errorMessage { get; set; }
    }
}
