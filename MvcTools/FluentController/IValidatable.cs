// MvcTools.FluentController.IViewModel.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools
{
    /// <summary>
    /// Provides a simple way to validate this object.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Is this view model valid?
        /// </summary>
        bool Validate();
    }
}
