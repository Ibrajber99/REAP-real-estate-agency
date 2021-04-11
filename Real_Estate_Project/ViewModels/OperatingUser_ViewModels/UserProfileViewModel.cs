using Real_Estate_Project.Models.Domain_Models.OperatingUser_Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Real_Estate_Project.ViewModels.OperatingUser_ViewModels
{
    public class UserProfileViewModel
    {
        public int UserID { get; set; }
        
        [Display(Name ="User name")]
        public string UserName { get; set; }

        [Display(Name ="Full name")]
        public string FullName { get; set; }

        [Display(Name ="Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "User Verified")]
        public bool IsVerified { get; set; }

        [Display(Name ="Street Address")]
        public string StreetAddress { get; set; }

        public string Municipality { get; set; }

        public string Province { get; set; }

        [Display(Name ="Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name ="Cell Phone")]
        public string CellPhone { get; set; }

        public bool IsTypeAgent { get; set;}

        [Display(Name ="Expiry Date. Minimum one year from to date")]
        public DateTime? LicenseExpiryDate { get; set; }

        public OperatingUserImage UserProfileImage { get; set; }

        public OperatingUserImage UserProfileDrivingLicense { get; set; }


        [Display(Name = "Profile Image")]
        public HttpPostedFileBase ProfileImageFile { get; set; }

        [Display(Name = "Profile Driver License")]
        public HttpPostedFileBase ProfileDriverLicense { get; set; }
    }
}