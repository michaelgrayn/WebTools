// MvcTools.FluentController.IViewModel.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace FluentController
{
    /// <summary>
    /// Provides a way to validate this view model and get the model it represents.
    /// </summary>
    /// <typeparam name="TViewModel">The type of object that is used by the fluent builder.</typeparam>
    public interface IViewModel<out TViewModel>
    {
        /// <summary>
        /// Converts this view model to a <typeparamref name="TViewModel" />.
        /// </summary>
        TViewModel Value();

        /// <summary>
        /// Is this view model valid?
        /// </summary>
        bool Valid();
    }
}
