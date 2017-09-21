// MvcTools.IValidatable.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools.FluentController
{
    /// <summary>
    /// Provides a simple way to validate this object.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Is this object valid?
        /// </summary>
        bool Validate();
    }
}
