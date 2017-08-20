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
        /// <returns>A fluent action.</returns>
        public FluentParameter([NotNull] TClient clientInput, bool firstErrorOnly = false)
        {
            _actionParameter = clientInput;
            var validationErrors = clientInput.Validate(new ValidationContext(clientInput));
            if (firstErrorOnly)
            {
                var validationResult = validationErrors.FirstOrDefault();
                _validationErrors = new List<ValidationResult>();
                if (validationResult != null) _validationErrors.Add(validationResult);
                return;
            }
            _validationErrors = validationErrors.ToList();
        }

        /// <summary>
        /// Sets the model returning action for the fluent action.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns></returns>
        public FluentAction<TClient, TModel> Action<TModel>([NotNull] Func<TClient, Task<TModel>> action)
        {
            return new FluentAction<TClient, TModel>(_actionParameter, _validationErrors, action);
        }
    }
}
