using Real_Estate_Project.Custome_Validations;
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models.Customer_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Real_Estate_Project.Models.Domain_Models
{
    public class Customer
    {
        public Customer()
        {
            dateOfBirth = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        public int AddressID { get; set; }

        [ForeignKey("AddressID")]
        public virtual CustomerAddress CustomerAddress { get; set; }

        [Required]
        [RegularExpression(@"^(\+\d{1,2}\s?)?1?\-?\.?\s?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$",
        ErrorMessage = "Please Provide a valid Phone number")]
        [Display(Name = "Cell Phone")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
          ErrorMessage ="Please provide a valid email")]
        public string Email { get; set; }

        [Required]
        [Min19YearsValidation]
        [DataType(DataType.DateTime)]
        [Display(Name ="Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dateOfBirth { get; set; }

        [Display(Name ="Created On")]
        public DateTime DateCreated { get; set; }

        [Display(Name ="Updated On")]
        public DateTime DateUpdated { get; set; }

        public bool IsActive { get; set; }

        public int? UserCreatorId { get; set; }
       
        public int? UserUpdatorId { get; set; }

        [ForeignKey("UserCreatorId")]
        [Display(Name ="Created By")]
        public  OperatingUser UserCreator { get; set; }

        [ForeignKey("UserUpdatorId")]
        [Display(Name ="Updated By")]
        public  OperatingUser UserUpdator { get; set; }

        [NotMapped]
        public string  FullName { get { return FirstName + " " + LastName; } private set { ;}}

        public  List<Listing> Listings { get; set; }

        public  List<Viewing> Viewings { get; set; }

    }
}