namespace Cint.CodingChallenge.Web.Extensions
{
    public static class HttpRequestExtensions
    {
        const string textHtmlHeader = "text/html";
        public static bool IsHtmlRequest(this HttpRequest request)
        {
            // This is a workaround for the fact that the Accept header is not mocked in the test AND 
            // the fact that the Accept header is set to */* in the request when using the Swagger UI.
            // So we are looking to see if it is an text/html accept header which is sent by the browser.
            var acceptHeader = request?.Headers["Accept"] ?? string.Empty;
            return acceptHeader.ToString().Contains(textHtmlHeader);
        }
    }
}
