using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Custome_Validations
{
    public class Min18YearsValidation : ValidationAttribute
    {
        private readonly DateTime CURRENT_DATE = DateTime.Today;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var age = 0;


            if (isOfTypeModel(validationContext))
            {
                var userDetails = validationContext.ObjectInstance as OperatingUser;
               age = CURRENT_DATE.Year - userDetails.DateOfBirth.Year;
            }

            if (isOfTypeInputModel(validationContext))
            {
                var userDetails = validationContext.ObjectInstance as OperatingUserInputModel;
                age = CURRENT_DATE.Year - userDetails.DateOfBirth.Year;
            }
            
            return (age >= 18)
                ? ValidationResult.Success :
                new ValidationResult("Age must be 19 years or older for authorization");
        }

        private bool isOfTypeModel(ValidationContext validationContext)
        {
            return  validationContext.ObjectInstance is OperatingUser;
        }
        private bool isOfTypeInputModel(ValidationContext validationContext)
        {
            return validationContext.ObjectInstance is OperatingUserInputModel;
        }
    }
}