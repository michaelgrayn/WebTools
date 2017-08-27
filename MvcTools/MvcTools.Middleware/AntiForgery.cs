using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MvcTools.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    internal class AntiForgery
    {
        private readonly RequestDelegate _next;

        public AntiForgery(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.Keys.Contains("X-Cancel-Request"))
            {
                context.Response.StatusCode = 500;
                return;
            }

            await _next.Invoke(context);

            if (context.Request.Headers.Keys.Contains("X-Transfer-By"))
            {
                context.Response.Headers.Add("X-Transfer-Success", "true");
            }
        }
    }
}
