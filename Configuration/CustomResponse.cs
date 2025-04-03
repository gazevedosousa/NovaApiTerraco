namespace TerracoDaCida.Configuration
{
    public class CustomResponse
    {
        public CustomResponse(int statusCode, bool data, string message)
        {
            statusCode = statusCode;
            data = data;
            errorMessage = message;
        }

        public int statusCode { get; set; }
        public bool data { get; set; }
        public string errorMessage { get; set; }
    }
}
