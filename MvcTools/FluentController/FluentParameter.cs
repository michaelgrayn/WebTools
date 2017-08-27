using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentController
{
    /// <summary>
    /// A builder for fluent controller actions.
    /// </summary>
    /// <typeparam name="TClient">The type of the input to the action method.</typeparam>
    public class FluentParameter<TClient> where TClient : IValidatableObject
    {
        /// <summary>
        /// The parameter to pass to the action method.
        /// </summary>
        private readonly TClient _actionParameter;

        /// <summary>
        /// Any errors that were found during validation.
        /// </summary>
        private readonly IList<ValidationResult> _validationErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentParameter{TClient}"/> class.
        /// </summary>
        /// <param name="clientInput">The client input.</param>
        /// <param name="firstErrorOnly">Stop validating after one error?</param>
        /// <param name="modelState">Is the model state valid?</param>
        /// <returns>A fluent action.</returns>
        internal FluentParameter([NotNull] TClient clientInput, bool firstErrorOnly, bool modelState)
        {
            _actionParameter = clientInput;
            var validationErrors = clientInput.Validate(new ValidationContext(clientInput));
            if (firstErrorOnly)
            {
                var validationResult = validationErrors.FirstOrDefault();
                _validationErrors = new List<ValidationResult>();
                if (validationResult != null) _validationErrors.Add(validationResult);
            }
            else _validationErrors = validationErrors.ToList();
            if (modelState) return;
            _validationErrors.Add(new ValidationResult("The model state is invalid."));
        }

        /// <summary>
        /// Sets the model returning action for the fluent builder.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns></returns>
        public FluentAction<TClient, TModel> Action<TModel>([NotNull] Func<TClient, Task<TModel>> action)
        {
            return new FluentAction<TClient, TModel>(_actionParameter, _validationErrors, action);
        }

        /// <summary>
        /// Adds an action to execute to this fluent builder. These actions will all run at the same time.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns></returns>
        public FluentAction<TClient, object> Action([NotNull] Func<TClient, Task> action)
        {
            return new FluentAction<TClient, object>(_actionParameter, _validationErrors, x => null).Action(action);
        }
    }
}
