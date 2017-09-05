// MvcTools.FluentController.FluentControllerBase.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace FluentController
{
    using System;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Mvc;
    using MvcTools.ResultTypes;

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
        /// Creates a <see cref="XmlResult" /> object that serializes the specified <paramref name="data" /> object to XML.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <param name="xmlAttributeOverrides">The <see cref="XmlAttributeOverrides" /> to be used.</param>
        /// <returns>
        /// The created <see cref="XmlResult" /> that serializes the specified <paramref name="data" /> to XML format for the response.
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
        /// <param name="action">This action takes no parameters and returns a <typeparamref name="TModel" />.</param>
        /// <returns>A fluent action.</returns>
        [NonAction]
        protected static FluentAction<NoInput, TModel> Action<TModel>([NotNull] Func<Task<TModel>> action)
        {
            return new FluentAction<NoInput, TModel>(new FluentParameter<NoInput>(new NoInput(), true), async x => await action());
        }

        /// <summary>
        /// Validates the client input.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model for the action.</param>
        /// <returns>A fluent action.</returns>
        [NonAction]
        protected FluentParameter<TViewModel> CheckRequest<TViewModel>(IViewModel<TViewModel> viewModel)
        {
            return new FluentParameter<TViewModel>(viewModel, ModelState.IsValid);
        }
    }
}
