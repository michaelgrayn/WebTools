using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using ResultTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FluentController
{
    /// <summary>
    /// The base class for fluent controllers
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        /// <summary>
        /// Creates a <see cref="XmlResult"/> object that serializes the specified
        /// <paramref name="data"/> object to XML.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <param name="xmlAttributeOverrides">The <see cref="XmlAttributeOverrides"/> to be used.</param>
        /// <returns>
        /// The created <see cref="XmlResult"/> that serializes the specified
        /// <paramref name="data"/> to XML format for the response.
        /// </returns>
        [NonAction]
        public XmlResult Xml(object data, XmlAttributeOverrides xmlAttributeOverrides = null)
        {
            return new XmlResult(data, xmlAttributeOverrides);
        }

        /// <summary>
        /// Validates the client input.
        /// </summary>
        /// <typeparam name="TClient">The type of the client input.</typeparam>
        /// <typeparam name="TModel">The type of the model for the success result.</typeparam>
        /// <param name="input">The client input.</param>
        /// <returns>A fluent action builder.</returns>
        [NonAction]
        protected FluentAction<TClient, TModel> CheckRequest<TClient, TModel>(TClient input) where TClient : IValidatableObject
        {
            return new FluentAction<TClient, TModel>().CheckRequest(input);
        }

        /// <summary>
        /// Sets the model returning action for the fluent action builer.
        /// </summary>
        /// <typeparam name="TModel">The type of the input for the success result.</typeparam>
        /// <param name="action">This action takes no parameters and returns a <typeparamref name="TModel"/>.</param>
        /// <returns>A fluent action builder.</returns>
        [NonAction]
        protected FluentAction<IValidatableObject, TModel> Action<TModel>([NotNull] Func<Task<TModel>> action)
        {
            return new FluentAction<IValidatableObject, TModel>().Action(async actionParameter => await action());
        }

        /// <summary>
        /// Adds an action to execute to this fluent builder.
        /// These actions will all run at the same time.
        /// </summary>
        /// <param name="action">This action takes no parameters and returns nothing.</param>
        /// <returns>A fluent action builder.</returns>
        [NonAction]
        protected FluentAction<IValidatableObject, object> Action([NotNull] Func<Task> action)
        {
            return new FluentAction<IValidatableObject, object>().Action(async actionParameter => await action());
        }

        /// <summary>
        /// Adds an action to execute to this fluent builder.
        /// These actions will all run at the same time.
        /// </summary>
        /// <typeparam name="TClient">The type of the client input.</typeparam>
        /// <param name="action">This action has one parameter and returns nothing.</param>
        /// <returns>A fluent action builder.</returns>
        [NonAction]
        protected FluentAction<TClient, object> Action<TClient>([NotNull] Func<TClient, Task> action) where TClient : IValidatableObject
        {
            return new FluentAction<TClient, object>().Action(action);
        }
    }
}
