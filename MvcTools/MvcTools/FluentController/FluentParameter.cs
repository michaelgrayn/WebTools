﻿// MvcTools.FluentParameter.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.FluentController
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    /// A builder for fluent controller actions.
    /// </summary>
    /// <typeparam name="TIn">The type of the input to the main action method.</typeparam>
    public sealed class FluentParameter<TIn>
    {
        /// <summary>
        /// The state of the model binding.
        /// </summary>
        private readonly bool _modelState;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentParameter{TClient}" /> class.
        /// </summary>
        /// <param name="parameter">The client input.</param>
        /// <param name="modelState">Is the model state valid?</param>
        internal FluentParameter(TIn parameter, bool modelState)
        {
            Parameter = parameter;
            _modelState = modelState;
        }

        /// <summary>
        /// Is the client input valid?
        /// </summary>
        public bool Valid
        {
            get
            {
                if (Parameter is IValidatable validatable) return _modelState && validatable.Validate();
                return _modelState;
            }
        }

        /// <summary>
        /// Gets the client input.
        /// </summary>
        internal TIn Parameter { get; }

        /// <summary>
        /// Sets the main action for the fluent builder.
        /// </summary>
        /// <typeparam name="TOut">The type of the input to the success method.</typeparam>
        /// <param name="action">The action to perform.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TIn, TOut> Action<TOut>([NotNull] Func<TIn, Task<TOut>> action)
        {
            return new FluentAction<TIn, TOut>(this, action);
        }

        /// <summary>
        /// Adds an action to execute to this fluent builder. These actions will all run at the same time.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <returns>A fluent action.</returns>
        public FluentAction<TIn, object> Action([NotNull] Func<TIn, Task> action)
        {
            return new FluentAction<TIn, object>(this, null).Action(action);
        }
    }
}
