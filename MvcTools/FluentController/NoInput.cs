// MvcTools.FluentController.NoInput.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools
{
    /// <summary>
    /// Represents the input for a request that has no client input.
    /// </summary>
    public class NoInput : IValidatable
    {
        /// <inheritdoc />
        public bool Validate() => true;
    }
}
