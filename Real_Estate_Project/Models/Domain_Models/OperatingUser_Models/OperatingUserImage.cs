using Real_Estate_Project.DataAccess.Data_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Models.Domain_Models.OperatingUser_Models
{
    public class OperatingUserImage
    {
        public OperatingUserImage()
        {
            IsArchived = false;
        }
        
        public int ID { get; set; }
        
        [Required]
        public string FileName { get; set; }
        
        [Required]
        public string ContentType { get; set; }
        
        [Required]
        public byte[] Content { get; set; }

        public UserFileType FileType { get; set; }

        [Required]
        public bool IsArchived { get; set; }

        public DateTime LicenseExpiryDate { get; set; }

        public int? UserCreatorId { get; set; }
        
        public int? UserUpdatorId { get; set; }
        
        [ForeignKey("UserCreatorId")]
        [Display(Name = "Created By")]
        public OperatingUser UserCreator { get; set; }
        
        [ForeignKey("UserUpdatorId")]
        [Display(Name = "Updated By")]
        public OperatingUser UserUpdator { get; set; }
        
        [Required]
        public DateTime DateAdded { get; set; }
        
        public DateTime? DateArchived { get; set; }

        public OperatingUser OperatingUser { get; set; }
    }
}