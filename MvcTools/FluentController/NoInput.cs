// MvcTools.FluentController.NoInput.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace FluentController
{
    /// <summary>
    /// Represents the input for a request that has no client input.
    /// </summary>
    public class NoInput : IViewModel<NoInput>
    {
        /// <inheritdoc />
        public NoInput Value() => this;

        /// <inheritdoc />
        public bool Valid() => true;
    }
}
