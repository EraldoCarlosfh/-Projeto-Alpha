using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Alpha.Framework.MediatR.EventSourcing.Validators.Attributes
{
    public class FullName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.GetType() != typeof(string))
                throw new TypeAccessException($"O tipo {value.GetType()} não é valido como entrada de validação {nameof(FullName)}, tipo permitido: string");

            var typedValue = (string)value;

            if (typedValue.Split(" ").Count() < 2)
                return new ValidationResult("O nome completo informado não é válido");

            return null;
        }
    }
}