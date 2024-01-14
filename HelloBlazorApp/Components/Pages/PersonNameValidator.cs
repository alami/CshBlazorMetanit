﻿using System.ComponentModel.DataAnnotations;

namespace HelloBlazorApp.Components.Pages
{   // чтобы имя пользователя не могло принимать определенные значения
    public class PersonNameValidator : ValidationAttribute  {
        string[] names;
        public PersonNameValidator(string[] names)  {
            this.names = names;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            if (names.Contains(value?.ToString()))
                return new ValidationResult("Некорректное имя!");            
            return ValidationResult.Success;
        }
    }
}
