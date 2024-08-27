using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace NET_FRAMEWORKS_EXAMEN_OPDRACHT.Data






{
    // This class represents a custom middleware to check for the presence of a specific cookie
    public class CookieMiddleware : IMiddleware
    {
        // The main method invoked for each HTTP request, processing the request and passing control to the next middleware
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Attempt to retrieve the value of the "UserCookie" from the incoming request's cookies
            var cookieValue = context.Request.Cookies["UserCookie"];

            // Check if the "UserCookie" is present and not empty
            if (!string.IsNullOrEmpty(cookieValue))
            {
                // If the cookie is present, set a flag in the HttpContext Items dictionary
                context.Items["CookiePresente"] = true;
            }
            else
            {
                // If the cookie is not present, set the flag to false
                context.Items["CookiePresente"] = false;
            }

            // Continue processing the request by passing control to the next middleware in the pipeline
            await next(context);
        }
    }
}
