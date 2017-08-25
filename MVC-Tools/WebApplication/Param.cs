using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// ReSharper disable All
namespace WebApplication
{
    public class Param : IValidatableObject
    {
        public int Index { get; set; }

        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Index < 1) result.Add(new ValidationResult($"{nameof(Index)} must be positive.", new List<string> { nameof(Index) }));
            return result;
        }
    }
}
