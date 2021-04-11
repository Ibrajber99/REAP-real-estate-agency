using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Custome_Validations
{
    public class SINValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string sinVal = "";
            if (isOfTypeModel(validationContext))
            {
                var userDetails = validationContext.ObjectInstance as OperatingUser;
                sinVal = userDetails.SocialSecurityNumber;
            }
            if (isOfTypeInputModel(validationContext))
            {
                var userDetails = validationContext.ObjectInstance as OperatingUserInputModel;
                sinVal = userDetails.SocialSecurityNumber;
            }

            if (sinVal.ToArray().All(c => Char.IsDigit(c))
                && sinVal.Length == 9)
                return ValidationResult.Success;
            else
                return new ValidationResult
                ("SIN is not valid. please check your input again");

        }

        private bool isOfTypeModel(ValidationContext validationContext)
        {
            return validationContext.ObjectInstance is OperatingUser;
        }
        private bool isOfTypeInputModel(ValidationContext validationContext)
        {
            return validationContext.ObjectInstance is OperatingUserInputModel;
        }
    }
}