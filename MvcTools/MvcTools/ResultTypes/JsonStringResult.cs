// MvcTools.JsonStringResult.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.ResultTypes
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// An action result which writes the given JSON to the result stream.
    /// </summary>
    public class JsonStringResult : ContentResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStringResult" /> class.
        /// </summary>
        /// <param name="json">The JSON to write to the result stream.</param>
        public JsonStringResult(string json)
        {
            Content = json;
            ContentType = "application/json; charset=utf-8";
        }

        /// <summary>
        /// Writes the given JSON to the result stream asynchronously.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        /// <returns>A task to await.</returns>
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (Content == null) return;
            context.HttpContext.Response.Clear();
            using (var streamWriter = new StreamWriter(context.HttpContext.Response.Body, Encoding.UTF8))
            {
                await streamWriter.WriteAsync(Content);
            }
        }
    }
}
