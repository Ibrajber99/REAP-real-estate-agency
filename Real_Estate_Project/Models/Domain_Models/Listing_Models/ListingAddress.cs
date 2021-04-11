using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Models.Listing_Models
{
    public class ListingAddress
    {

        public int ID { get; set; }


        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "Municipality")]
        public string Municipality { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [RegularExpression(@"^([a-zA-Z]\d[a-zA-Z]( )?\d[a-zA-Z]\d)$",
            ErrorMessage = "Please provide a valid Postal code")]
        public string PostalCode { get; set; }

        [Display(Name = "Additional details")]
        public string AddressDetails { get; set; }
    }
}