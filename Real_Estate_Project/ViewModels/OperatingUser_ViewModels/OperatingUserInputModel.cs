using Real_Estate_Project.Custome_Validations;
using Real_Estate_Project.DataAccess.Data_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.ViewModels
{
    //ViewModel strictly for Modifying a user
    [NotMapped]
    public class OperatingUserInputModel 
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Social Security Number (SIN)")]
        [SINValidation]
        public string SocialSecurityNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public bool IsVerified { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z]\d[a-zA-Z]( )?\d[a-zA-Z]\d)$")]
        public string PostalCode { get; set; }

        public string HomePhone { get; set; }

        [Required]
        [RegularExpression(@"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")]
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Display(Name = "Office phone")]
        [RegularExpression(@"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")]
        public string OfficePhoneNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Min19YearsValidation]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Role Type")]
        public string RoleID { get; set; }

        [Display(Name = "Registered on")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Updated on")]
        public DateTime UpdatedDate { get; set; }

        public int? UserCreatorId { get; set; }

        public int? UserUpdatorId { get; set; }
    }
}