using Real_Estate_Project.Custome_Validations;
using Real_Estate_Project.Models.Domain_Models.OperatingUser_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Real_Estate_Project.DataAccess.Data_Models
{
    public class OperatingUser 
    {
        public OperatingUser()
        {
            DriverLicenses = new List<OperatingUserImage>();
            DateOfBirth = DateTime.Now;
        }


        #region Personal Information
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="Social Security Number (SIN)")]
        [SINValidation]
        public string SocialSecurityNumber { get; set; }

        [NotMapped]
        public string MaskedSINnum { get {
                return "******" + SocialSecurityNumber.
                    Substring(SocialSecurityNumber.Length-3);

            } set { }
        }

        [Required]
        [Display(Name ="First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Last name")]
        public string LastName { get; set; }

        [Display(Name ="Middle name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name ="Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z]\d[a-zA-Z]( )?\d[a-zA-Z]\d)$",
            ErrorMessage ="Please provide a valid Postal code")]
        public string PostalCode { get; set; }

        public string HomePhone { get; set; }

        [Required]
        [RegularExpression(@"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$",
            ErrorMessage ="Please provide a valid Phone number")]
        [Display(Name ="Cell Phone")]
        public string CellPhone { get; set; }

        [Display(Name ="Office phone")]
        [RegularExpression(@"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$",
            ErrorMessage ="Please provide a valid number")]
        public string OfficePhoneNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Min19YearsValidation]
        [Display(Name ="Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Role Type")]
        public string RoleID { get; set; }

        public bool IsActive { get; set; }

        [Display(Name ="User Verified")]
        public bool IsVerified { get; set; }
        #endregion


        public OperatingUserImage ProfileImage { get; set; }


        public List<OperatingUserImage> DriverLicenses { get; set; }


        [NotMapped]
        public OperatingUserImage CurrentDrivingLicense =>
                DriverLicenses.LastOrDefault(x => !x.IsArchived);


        #region Dates and Relationships
        [Display(Name ="Registered on")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Updated on")]
        public DateTime UpdatedDate { get; set; }

        public int? UserCreatorId { get; set; }
       
        public int? UserUpdatorId { get; set; }

        [ForeignKey("UserCreatorId")]
        [Display(Name ="Created By")]
        public  OperatingUser UserCreator { get; set; }

        [ForeignKey("UserUpdatorId")]
        [Display(Name = "Updated By")]
        public  OperatingUser UserUpdator { get; set; }

        public  List<Viewing> Viewings { get; set; }
        #endregion


        [NotMapped]
        public string FullName 
            { get {return FirstName +" " + LastName; }
            private set {; } }
    }
}

