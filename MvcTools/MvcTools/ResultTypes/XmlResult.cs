// MvcTools.XmlResult.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.ResultTypes
{
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents a class that is used to send XML-formatted content to the response.
    /// </summary>
    public sealed class XmlResult : ActionResult
    {
        /// <summary>
        /// The object to be serialized to XML.
        /// </summary>
        private readonly object _data;

        /// <summary>
        /// The serializer to use on the object.
        /// </summary>
        private readonly XmlSerializer _xmlSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult" /> class.
        /// </summary>
        /// <param name="data">The object to serialize to XML.</param>
        /// <param name="xmlSerializer"><see cref="XmlSerializer" /> to use.</param>
        public XmlResult(object data, XmlSerializer xmlSerializer = null)
        {
            _data = data;
            _xmlSerializer = xmlSerializer ?? new XmlSerializer(_data.GetType());
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the
        /// corresponding XML to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ActionContext context)
        {
            if (_data == null) return;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = "application/xml; charset=utf-8";
            using (var xmlWriter = new XmlTextWriter(context.HttpContext.Response.Body, Encoding.UTF8))
            {
                _xmlSerializer.Serialize(xmlWriter, _data);
            }
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the
        /// corresponding XML to the result stream asynchronously.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        /// <returns>A task to await.</returns>
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await Task.Run(() => ExecuteResult(context));
        }
    }
}
