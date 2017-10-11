// MvcTools.JsonStringResult.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.ResultTypes
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// An action result which writes the given JSON to the result stream.
    /// </summary>
    public class JsonStringResult : ActionResult
    {
        /// <summary>
        /// The JSON to write to the result stream.
        /// </summary>
        private readonly string _json;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStringResult" /> class.
        /// </summary>
        /// <param name="json">The JSON to write to the result stream.</param>
        public JsonStringResult(string json)
        {
            _json = json;
        }

        /// <summary>
        /// Writes the given JSON to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ActionContext context)
        {
            if (_json == null) return;
            ClearResponseAndSetContentType(context);
            using (var streamWriter = new StreamWriter(context.HttpContext.Response.Body))
            {
                streamWriter.Write(_json);
            }
        }

        /// <summary>
        /// Writes the given JSON to the result stream asynchronously.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        /// <returns>A task to await.</returns>
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (_json == null) return;
            ClearResponseAndSetContentType(context);
            using (var streamWriter = new StreamWriter(context.HttpContext.Response.Body))
            {
                await streamWriter.WriteAsync(_json);
            }
        }

        /// <summary>
        /// Clears any existing content and sets the content type.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        private static void ClearResponseAndSetContentType(ActionContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = "application/json; charset=utf-8";
        }
    }
}
