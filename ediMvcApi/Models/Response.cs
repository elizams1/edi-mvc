using System.Net;

namespace ediMvcApi.Models
{
    public class Response
    {
        public HttpStatusCode statusCode { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }

        public Response()
        {
            statusCode = HttpStatusCode.InternalServerError;
            message = "";
            data = null;
        }
    }
}
