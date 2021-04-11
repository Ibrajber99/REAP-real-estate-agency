using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models.Domain_Models.Listing_Models
{
    public class ListingImage
    {
        public ListingImage()
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

        public byte[] ThumbnailContent {get; set;}

        [Required]
        public bool IsArchived { get; set; }

        public bool IsDefaultImage { get; set; }

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

        public Listing Listing { get; set; }

    }
}