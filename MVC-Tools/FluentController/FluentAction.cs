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
        private Func<TClient, Task<TModel>> _actionModel;

        /// <summary>
        /// The parameter to pass to the action method.
        /// </summary>
        private TClient _actionParameter;

        /// <summary>
        /// What to do on action failure.
        /// </summary>
        private Func<Exception, IEnumerable<ValidationResult>, IActionResult> _error;
        
        /// <summary>
        /// What to do on action success.
        /// </summary>
        private Func<TModel, IActionResult> _success;

        /// <summary>
        /// The other tasks to run.
        /// </summary>
        private List<Func<Task>> _taskList = new List<Func<Task>>();

        /// <summary>
        /// Any errors that were found during validation.
        /// </summary>
        private List<ValidationResult> _validationErrors = new List<ValidationResult>();

        /// <summary>
        /// The error result to use if ther isn't one specified.
        /// </summary>
        public IActionResult DefaultError { get; set; } = new EmptyResult();

        /// <summary>
        /// The success result to use if there isn't one specified.
        /// </summary>
        public IActionResult DefaultSuccess { get; set; } = new BadRequestResult();

        /// <summary>
        /// Sets the model returning action for the fluent builder.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TClient, TModel> Action([NotNull] Func<TClient, Task<TModel>> action)
        {
            _actionModel = action;
            return this;
        }
        
        /// <summary>
        /// Adds an action to execute to this fluent builder.
        /// These actions will all run at the same time.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TClient, TModel> Action([NotNull] Func<TClient, Task> action)
        {
            _taskList.Add(async () => await action(_actionParameter));
            return this;
        }

        /// <summary>
        /// Validates the client input.
        /// </summary>
        /// <param name="clientInput">The client input.</param>
        /// <param name="firstErrorOnly">Stop validating after one error?</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TClient, TModel> CheckRequest([NotNull] TClient clientInput, bool firstErrorOnly = false)
        {
            _actionParameter = clientInput;
            var validationResults = clientInput.Validate(new ValidationContext(clientInput));
            if (firstErrorOnly)
            {
                var validationResult = validationResults.FirstOrDefault();
                if (validationResult == null) return this;
                _validationErrors.Add(validationResult);
                return this;
            }
            _validationErrors = validationResults.ToList();
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
        /// Performs the action(s) and returns a result.
        /// If there is no action that returns a <typeparamref name="TModel"/> then default(<typeparamref name="TModel"/>) will be used for the success result.
        /// </summary>
        /// <returns>The resulting response to the request.</returns>
        public async Task<IActionResult> ResponseAsync()
        {
            try
            {
                if (_validationErrors.Any()) return ErrorInvoker();

                foreach (var task in _taskList.Select(task => task.Invoke()))
                {
                    await task;
                }

                var model = await (_actionModel?.Invoke(_actionParameter) ?? Task.FromResult(default(TModel)));
                return _success?.Invoke(model) ?? DefaultSuccess;
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
            return _error?.Invoke(exception, _validationErrors) ?? DefaultError;
        }
    }
}
