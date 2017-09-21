// WebApplication.Param.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace WebApplication
{
    using System.ComponentModel.DataAnnotations;
    using MvcTools.FluentController;

    public class Param : IValidatable
    {
        [Range(0, int.MaxValue)]
        public int Index { get; set; }

        public string Value { get; set; }

        public bool Validate()
        {
            return Index >= 0;
        }
    }
}
