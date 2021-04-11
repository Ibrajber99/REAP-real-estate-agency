using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Custome_Validations
{
    public class Min19YearsValidation : ValidationAttribute
    {
        private readonly DateTime CURRENT_DATE = DateTime.Today;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var age = 0;
            DateTime currDate = DateTime.Now;

            if (isOfTypeUserModel(validationContext))
            {
                var userDetails = validationContext.ObjectInstance as OperatingUser;
                currDate = userDetails.DateOfBirth;
                age = CURRENT_DATE.Year - userDetails.DateOfBirth.Year;
            }

            if (isOfTypeUserInputModel(validationContext))
            {
                var userDetails = validationContext.ObjectInstance as OperatingUserInputModel;
                currDate = userDetails.DateOfBirth;
                age = CURRENT_DATE.Year - userDetails.DateOfBirth.Year;
            }

            if (isOfTypeCustomer(validationContext))
            {
                var customerDetails = validationContext.ObjectInstance as Customer;
                currDate = customerDetails.dateOfBirth;
                age = CURRENT_DATE.Year - customerDetails.dateOfBirth.Year;
            }


            if (currDate.Year < 1920)
                return new ValidationResult("Year of birth must be bigger or equal to 1920");

            return (age >= 19)
                ? ValidationResult.Success :
                new ValidationResult("Age must be 19 years or older for authorization");
        }

        private bool isOfTypeUserModel(ValidationContext validationContext)
        {
            return  validationContext.ObjectInstance is OperatingUser;
        }

        private bool isOfTypeUserInputModel(ValidationContext validationContext)
        {
            return validationContext.ObjectInstance is OperatingUserInputModel;
        }

        private bool isOfTypeCustomer(ValidationContext validationContext)
        {
            return validationContext.ObjectInstance is Customer;
        }
    }
}