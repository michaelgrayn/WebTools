// MvcTools.FluentController.FluentParameter.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace FluentController
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    /// A builder for fluent controller actions.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the input to the action method.</typeparam>
    public class FluentParameter<TViewModel>
    {
        private readonly bool _modelState;

        /// <summary>
        /// The parameter to pass to the action method.
        /// </summary>
        private readonly IViewModel<TViewModel> _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentParameter{TClient}" /> class.
        /// </summary>
        /// <param name="clientInput">The client input.</param>
        /// <param name="modelState">Is the model state valid?</param>
        /// <returns>A fluent action.</returns>
        internal FluentParameter([NotNull] IViewModel<TViewModel> clientInput, bool modelState)
        {
            _viewModel = clientInput;
            _modelState = modelState;
        }

        /// <summary>
        /// Is the client input valid?
        /// </summary>
        public bool Valid => _modelState && _viewModel.Valid();

        /// <summary>
        /// Gets the view model.
        /// </summary>
        internal TViewModel ViewModel => _viewModel.Value();

        /// <summary>
        /// Sets the model returning action for the fluent builder.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TViewModel, TModel> Action<TModel>([NotNull] Func<TViewModel, Task<TModel>> action)
        {
            return new FluentAction<TViewModel, TModel>(this, action);
        }

        /// <summary>
        /// Adds an action to execute to this fluent builder. These actions will all run at the same time.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TViewModel, object> Action([NotNull] Func<TViewModel, Task> action)
        {
            return new FluentAction<TViewModel, object>(this, x => null).Action(action);
        }
    }
}
