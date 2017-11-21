// MvcTools.XmlResult.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.ResultTypes
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a class that is used to send XML-formatted content to the response.
    /// </summary>
    public sealed class XmlResult : ContentEncoder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResult" /> class.
        /// </summary>
        /// <param name="data">The object to serialize to XML.</param>
        /// <param name="xmlSerializer"><see cref="XmlSerializer" /> to use.</param>
        public XmlResult(object data, XmlSerializer xmlSerializer = null)
        {
            xmlSerializer = xmlSerializer ?? new XmlSerializer(data.GetType());
            ContentType = "application/xml; charset=utf-8";

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, data);
                Content = stringWriter.ToString();
            }
        }
    }
}
