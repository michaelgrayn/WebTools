// MvcTools.WebApplication.Param.cs
// By Matthew DeJonge
// Email: mhdejong@umich.edu

namespace WebApplication
{
    using System.ComponentModel.DataAnnotations;
    using FluentController;

    public class Param : IViewModel<Param>
    {
        [Range(0, int.MaxValue)]
        public int Index { get; set; }

        public string Value { get; set; }

        Param IViewModel<Param>.Value() => this;

        bool IViewModel<Param>.Valid() => Index >= 0;
    }
}
