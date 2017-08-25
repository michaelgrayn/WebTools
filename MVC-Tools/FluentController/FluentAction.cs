using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
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
    /// <typeparam name="TModel">The type of the input for the success method.</typeparam>
    public class FluentAction<TClient, TModel> where TClient : IValidatableObject
    {
        /// <summary>
        /// The action to perform that returns a model for the success result.
        /// </summary>
        private readonly Func<TClient, Task<TModel>> _actionModel;

        /// <summary>
        /// The parameter to pass to the action method.
        /// </summary>
        private readonly TClient _actionParameter;

        /// <summary>
        /// The other tasks to run.
        /// </summary>
        private readonly IList<Func<TClient, Task>> _taskList = new List<Func<TClient, Task>>();

        /// <summary>
        /// Any errors that were found during validation.
        /// </summary>
        private readonly IList<ValidationResult> _validationErrors;

        /// <summary>
        /// What to do on action failure.
        /// </summary>
        private Func<Exception, IEnumerable<ValidationResult>, IActionResult> _error;

        /// <summary>
        /// What to do on action success.
        /// </summary>
        private Func<TModel, IActionResult> _success;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentAction{TClient, TModel}"/> class.
        /// </summary>
        public FluentAction()
        {
            _validationErrors = new List<ValidationResult>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentAction{TClient, TModel}"/> class.
        /// </summary>
        /// <param name="input">The client input.</param>
        /// <param name="validationResults">The results from validating the client input.</param>
        /// <param name="action">The action to perform, which returns the model.</param>
        internal FluentAction(TClient input, IList<ValidationResult> validationResults, Func<TClient, Task<TModel>> action)
        {
            _actionParameter = input;
            _validationErrors = validationResults;
            _actionModel = action;
        }

        /// <summary>
        /// Adds an action to execute to this fluent builder.
        /// These actions will all run at the same time.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TClient, TModel> Action([NotNull] Func<TClient, Task> action)
        {
            _taskList.Add(action);
            return this;
        }

        /// <summary>
        /// Sets an error method for the fluent builder.
        /// </summary>
        /// <param name="error">What to do if the action fails.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TClient, TModel> Error(Func<Exception, IEnumerable<ValidationResult>, IActionResult> error)
        {
            _error = error;
            return this;
        }

        /// <summary>
        /// Performs the action(s) and returns a result. The action that returns the model will run before all other tasks.
        /// </summary>
        /// <returns>The resulting response to the request.</returns>
        public async Task<IActionResult> ResponseAsync()
        {
            try
            {
                if (_validationErrors.Any()) return ErrorInvoker();

                if (_actionModel == null) throw new InvalidOperationException($"{nameof(FluentParameter<TClient>.Action)} must be called prior to calling {nameof(ResponseAsync)}.");
                var model = await _actionModel(_actionParameter);

                // The ToList() call is important for ensuring that the tasks run simultaneously.
                var tasks = _taskList.Select(task => task(_actionParameter)).ToList();
                foreach (var task in tasks)
                {
                    await task;
                }

                return _success?.Invoke(model) ?? FluentControllerBase.DefaultSuccess;
            }
            catch (Exception e)
            {
                return ErrorInvoker(e);
            }
        }

        /// <summary>
        /// Sets a success method for the fluent builder.
        /// </summary>
        /// <param name="success">What to do if the action succeeds.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TClient, TModel> Success(Func<TModel, IActionResult> success)
        {
            _success = success;
            return this;
        }

        /// <summary>
        /// Performs the error action.
        /// </summary>
        /// <param name="exception">An exception that was caught during execution of the action.</param>
        /// <returns>An error result.</returns>
        private IActionResult ErrorInvoker(Exception exception = null)
        {
            return _error?.Invoke(exception, _validationErrors) ?? FluentControllerBase.DefaultError;
        }
    }
}
