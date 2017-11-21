namespace MvcTools.ResultTypes
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Encodes content in a specified encoding.
    /// </summary>
    public class ContentEncoder : ContentResult
    {
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <inheritdoc />
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Clear();
            using (var streamWriter = new StreamWriter(context.HttpContext.Response.Body, Encoding))
            {
                await streamWriter.WriteAsync(Content);
            }
        }
    }
}
