// MvcTools.FluentController.FluentAction.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace FluentController
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
    /// <typeparam name="TViewModel">The type of the view model for the action.</typeparam>
    /// <typeparam name="TModel">The type of the input for the success method.</typeparam>
    public class FluentAction<TViewModel, TModel>
    {
        /// <summary>
        /// The action to perform that returns a model for the success result.
        /// </summary>
        private readonly Func<TViewModel, Task<TModel>> _actionModel;

        /// <summary>
        /// The other tasks to run.
        /// </summary>
        private readonly IList<Func<TViewModel, Task>> _taskList = new List<Func<TViewModel, Task>>();

        /// <summary>
        /// The parameter to pass to the action method.
        /// </summary>
        private readonly FluentParameter<TViewModel> _viewModel;

        /// <summary>
        /// What to do on action failure.
        /// </summary>
        private Func<Exception, IActionResult> _error;

        /// <summary>
        /// What to do on action success.
        /// </summary>
        private Func<TModel, IActionResult> _success;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentAction{TClient, TModel}" /> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="action">The action to perform, which returns the model.</param>
        internal FluentAction(FluentParameter<TViewModel> viewModel, Func<TViewModel, Task<TModel>> action)
        {
            _viewModel = viewModel;
            _actionModel = action;
        }

        /// <summary>
        /// Adds an action to execute to this fluent builder. These actions will all run at the same time.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TViewModel, TModel> Action([NotNull] Func<TViewModel, Task> action)
        {
            _taskList.Add(action);
            return this;
        }

        /// <summary>
        /// Sets an error method for the fluent builder.
        /// </summary>
        /// <param name="error">What to do if the action fails.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TViewModel, TModel> Error(Func<Exception, IActionResult> error)
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
                if (!_viewModel.Valid) return ErrorInvoker();

                TModel model;
                if (_actionModel == null) model = default(TModel);
                else model = await _actionModel(_viewModel.ViewModel);

                // The ToList() call is important for ensuring that the tasks run simultaneously.
                var tasks = _taskList.Select(task => task(_viewModel.ViewModel)).ToList();
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
        public FluentAction<TViewModel, TModel> Success(Func<TModel, IActionResult> success)
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
