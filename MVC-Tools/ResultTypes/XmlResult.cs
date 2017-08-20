using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ResultTypes
{
    /// <summary>
    /// Represents a class that is used to send XML-formatted content to the response.
    /// </summary>
    public class XmlResult : ActionResult
    {
        /// <summary>
        /// The object to be serialized to XML.
        /// </summary>
        private readonly object _objectToSerialize;

        /// <summary>
        /// The serializer to use on the object.
        /// </summary>
        private readonly XmlSerializer _xmlSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult"/> class.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize to XML.</param>
        /// <param name="attributeOverrides"><see cref="XmlAttributeOverrides"/> to use during serialization.</param>
        public XmlResult(object objectToSerialize, [CanBeNull] XmlAttributeOverrides attributeOverrides)
        {
            _objectToSerialize = objectToSerialize;
            _xmlSerializer = attributeOverrides == null ?
                new XmlSerializer(_objectToSerialize.GetType()) : 
                new XmlSerializer(_objectToSerialize.GetType(), attributeOverrides);
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override void ExecuteResult(ActionContext context)
        {
            if (_objectToSerialize == null) return;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = "application/xml; charset=utf-8";
            using (var xmlWriter = new XmlTextWriter(context.HttpContext.Response.Body, Encoding.UTF8))
            {
                _xmlSerializer.Serialize(xmlWriter, _objectToSerialize);
            }
        }

        /// <summary>
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream asynchronously.
        /// </summary>
        /// <param name="context">The controller context for the current request.</param>
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await Task.Run(() => ExecuteResult(context));
        }
    }
}