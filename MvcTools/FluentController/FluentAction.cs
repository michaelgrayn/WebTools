// MvcTools.FluentController.FluentAction.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// A builder for fluent controller actions.
    /// </summary>
    /// <typeparam name="TIn">The type of the client input to the action.</typeparam>
    /// <typeparam name="TOut">The type of the input for the success method.</typeparam>
    public sealed class FluentAction<TIn, TOut> where TIn : IValidatable
    {
        /// <summary>
        /// The main action to perform. Returns a model for the success method.
        /// </summary>
        private readonly Func<TIn, Task<TOut>> _mainAction;

        /// <summary>
        /// The parameter to pass to the main action method.
        /// </summary>
        private readonly FluentParameter<TIn> _parameter;

        /// <summary>
        /// The other tasks to run.
        /// </summary>
        private readonly IList<Func<TIn, Task>> _taskList = new List<Func<TIn, Task>>();

        /// <summary>
        /// What to do on action failure.
        /// </summary>
        private Func<Exception, IActionResult> _error;

        /// <summary>
        /// What to do on action success.
        /// </summary>
        private Func<TOut, IActionResult> _success;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentAction{TClient, TModel}" /> class.
        /// </summary>
        /// <param name="parameter">The view model.</param>
        /// <param name="mainAction">The main action to perform.</param>
        internal FluentAction(FluentParameter<TIn> parameter, Func<TIn, Task<TOut>> mainAction)
        {
            _parameter = parameter;
            _mainAction = mainAction;
        }

        /// <summary>
        /// Adds an action to execute to this fluent builder. These actions will all run at the same time.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TIn, TOut> Action([NotNull] Func<TIn, Task> action)
        {
            _taskList.Add(action);
            return this;
        }

        /// <summary>
        /// Sets an error method for the fluent builder.
        /// </summary>
        /// <param name="error">What to do if the action fails.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TIn, TOut> Error(Func<Exception, IActionResult> error)
        {
            _error = error;
            return this;
        }

        /// <summary>
        /// Performs the action(s) and returns a result. The main action will run before all other tasks.
        /// </summary>
        /// <returns>The resulting response to the request.</returns>
        public async Task<IActionResult> ResponseAsync()
        {
            try
            {
                if (!_parameter.Valid) return ErrorInvoker();

                TOut model;
                if (_mainAction == null) model = default(TOut);
                else model = await _mainAction(_parameter.Parameter);

                // The ToList() call is important for ensuring that the tasks run simultaneously.
                var tasks = _taskList.Select(task => task(_parameter.Parameter)).ToList();
                foreach (var task in tasks) await task;

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
        public FluentAction<TIn, TOut> Success(Func<TOut, IActionResult> success)
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
            return _error?.Invoke(exception) ?? FluentControllerBase.DefaultError;
        }
    }
}
