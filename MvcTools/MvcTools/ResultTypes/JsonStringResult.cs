// MvcTools.JsonStringResult.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.ResultTypes
{
    /// <summary>
    /// An action result which writes the given JSON to the result stream.
    /// </summary>
    public class JsonStringResult : EncodedContentResult
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
    }
}
