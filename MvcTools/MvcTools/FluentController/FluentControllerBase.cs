﻿// MvcTools.FluentControllerBase.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.FluentController
{
    using System;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Mvc;
    using MvcTools.ResultTypes;

    /// <summary>
    /// The base class for fluent controllers.
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
        /// Creates a <see cref="JsonStringResult" /> object that writes the specified <paramref name="json" /> to the result stream.
        /// </summary>
        /// <param name="json">The JSON to write to the result stream.</param>
        /// <returns>The created <see cref="JsonStringResult" /> that writes the specified <paramref name="json" /> to the result stream.</returns>
        [NonAction]
        public JsonStringResult JsonString(string json)
        {
            return new JsonStringResult(json);
        }

        /// <summary>
        /// Creates an <see cref="XmlResult" /> object that serializes the specified <paramref name="data" /> object to XML.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <returns>
        /// The created <see cref="XmlResult" /> that serializes the specified <paramref name="data" /> to XML format for the response.
        /// </returns>
        [NonAction]
        public XmlResult Xml(object data)
        {
            return new XmlResult(data);
        }

        /// <summary>
        /// Creates an <see cref="XmlResult" /> object that serializes the specified <paramref name="data" /> object to XML.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <param name="xmlSerializer"><see cref="XmlSerializer" /> to use.</param>
        /// <returns>
        /// The created <see cref="XmlResult" /> that serializes the specified <paramref name="data" /> to XML format for the response.
        /// </returns>
        [NonAction]
        public XmlResult Xml(object data, XmlSerializer xmlSerializer)
        {
            return new XmlResult(data, xmlSerializer);
        }

        /// <summary>
        /// Sets the main action for the fluent builder.
        /// </summary>
        /// <typeparam name="TOut">The type of the input for the success result.</typeparam>
        /// <param name="action">This action takes no parameters and returns a <typeparamref name="TOut" />.</param>
        /// <returns>A fluent action.</returns>
        protected static FluentAction<object, TOut> Action<TOut>([NotNull] Func<Task<TOut>> action)
        {
            return new FluentAction<object, TOut>(new FluentParameter<object>(null, true), async x => await action());
        }

        /// <summary>
        /// Validates the client input.
        /// </summary>
        /// <typeparam name="TIn">The type of the client input.</typeparam>
        /// <param name="parameter">The input to the action.</param>
        /// <returns>A fluent action.</returns>
        protected FluentParameter<TIn> RequestParameter<TIn>(TIn parameter)
        {
            return new FluentParameter<TIn>(parameter, ModelState.IsValid);
        }
    }
}
