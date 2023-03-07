using Ganss.Xss;
using System.Text;

namespace NoXss.API
{
    /// <summary>
    /// from https://jason.sultana.net.au/dotnet/security/apis/2021/09/26/preventing-xss-in-netcore-webapi.html
    /// </summary>
    public class AntiXssMiddleware
    {
        private readonly RequestDelegate _next;

        public AntiXssMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // enable buffering so that the request can be read by the model binders next
            httpContext.Request.EnableBuffering();

            // leaveOpen: true to leave the stream open after disposing, so it can be read by the model binders
            using (var streamReader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var raw = await streamReader.ReadToEndAsync();
                var sanitiser = new HtmlSanitizer(); //https://github.com/mganss/HtmlSanitizer
                var sanitised = sanitiser.Sanitize(raw);

                if (raw != sanitised)
                {
                    throw new BadHttpRequestException("XSS injection detected from middleware.");
                }
            }

            // rewind the stream for the next middleware
            _ = httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            await _next.Invoke(httpContext);
        }
    }
}
