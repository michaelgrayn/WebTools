using Microsoft.AspNetCore.Builder;

namespace MvcTools.Middleware
{
    /// <summary>
    /// Extension methods for adding typed middleware.
    /// </summary>
    public static class UseMiddlewareExtensions
    {
        /// <summary>
        /// Adds request forgery protection to the <see
        /// cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseAntiForgery(this IApplicationBuilder app)
        {
            app.UseMiddleware<AntiForgery>();
            return app;
        }
    }
}
