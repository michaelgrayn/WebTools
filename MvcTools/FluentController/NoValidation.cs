// MvcTools.FluentController.NoInput.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace MvcTools
{
    /// <summary>
    /// Represents the input for a request that has no client input.
    /// This class can also be inherited to handle empty validation.
    /// </summary>
    public class NoValidation : IValidatable
    {
        /// <inheritdoc />
        public bool Validate()
        {
            return true;
        }
    }
}
