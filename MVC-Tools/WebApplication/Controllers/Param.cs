using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Controllers
{
    public class Param : IValidatableObject
    {
        /// <summary>
        /// Int must be positive.
        /// </summary>
        public int Int { get; set; }

        /// <summary>
        /// A value.
        /// </summary>
        public string Value { get; set; }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            if (Int < 1) result.Add(new ValidationResult($"{nameof(Int)} must be positive.", new List<string> { nameof(Int) }));
            return result;
        }
    }
}