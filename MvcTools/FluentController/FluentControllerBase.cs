using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using MvcTools.ResultTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FluentController
{
    /// <summary>
    /// The base class for fluent controllers
    /// </summary>
    public abstract class FluentControllerBase : Controller
    {
        /// <summary>
        /// The error result to use if ther isn't one specified.
        /// </summary>
        public static IActionResult DefaultError { get; set; } = new BadRequestResult();

        /// <summary>
        /// The success result to use if there isn't one specified.
        /// </summary>
        public static IActionResult DefaultSuccess { get; set; } = new EmptyResult();

        /// <summary>
        /// Creates a <see cref="XmlResult"/> object that serializes the specified
        /// <paramref name="data"/> object to XML.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <param name="xmlAttributeOverrides">The <see cref="XmlAttributeOverrides"/> to be used.</param>
        /// <returns>
        /// The created <see cref="XmlResult"/> that serializes the specified <paramref name="data"/>
        /// to XML format for the response.
        /// </returns>
        [NonAction]
        public XmlResult Xml(object data, XmlAttributeOverrides xmlAttributeOverrides = null)
        {
            return new XmlResult(data, xmlAttributeOverrides);
        }

        /// <summary>
        /// Sets the model returning action for the fluent builder.
        /// </summary>
        /// <typeparam name="TModel">The type of the input for the success result.</typeparam>
        /// <param name="action">This action takes no parameters and returns a <typeparamref name="TModel"/>.</param>
        /// <returns>A fluent action builder.</returns>
        [NonAction]
        protected static FluentAction<object, TModel> Action<TModel>([NotNull] Func<Task<TModel>> action)
        {
            return new FluentAction<object, TModel>(null, new List<ValidationResult>(), async actionParameter => await action());
        }

        /// <summary>
        /// Validates the client input.
        /// </summary>
        /// <typeparam name="TClient">The type of the client input.</typeparam>
        /// <param name="input">The client input.</param>
        /// <param name="firstErrorOnly">Stop validation after the first error?</param>
        /// <returns>A fluent action builder.</returns>
        [NonAction]
        protected FluentParameter<TClient> CheckRequest<TClient>(TClient input, bool firstErrorOnly = false) where TClient : IValidatableObject
        {
            return new FluentParameter<TClient>(input, firstErrorOnly, ModelState.IsValid);
        }
    }
}
