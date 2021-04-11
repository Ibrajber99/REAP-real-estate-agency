using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Listing_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace Real_Estate_Project.Models.Listing_Models
{
    public class Listing
    {
        public Listing()
        {
            Features = new List<PropertyFeatures>();
            Heating = new List<Heating>();
            ImagesContent = new List<ListingImage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        [Display(Name = "Customer")]
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [Display(Name ="Agent")]
        public int AssociatedAgentID { get; set; }

        [ForeignKey("AssociatedAgentID")]
        [Display(Name ="Agent")]
        public  virtual OperatingUser AssociatedAgent { get; set; }

        [Required]
        [Display(Name ="Property Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Range(500, int.MaxValue,ErrorMessage ="Please provide aminimum value of 500")]
        public decimal Price { get; set; }

        public int ListingAddressID { get; set; }

        [ForeignKey("ListingAddressID")]
        public virtual ListingAddress ListingAddress { get; set; }

        [Required]
        [Display(Name ="Square Footage")]
        [Range(120, int.MaxValue, ErrorMessage = "Please provide aminimum value of 500")]
        public decimal SquareFootage { get; set; }

        [Required]
        [Display(Name ="Number of Beds")]
        public int NumOfBeds { get; set; }

        [Required]
        [Display(Name ="Number of Baths")]
        public int NumOfBaths { get; set; }

        [Required]
        [Display(Name ="Number of Stories")]
        public int NumOfStories { get; set; }

        [Required]
        [Display(Name ="City Area")]
        public string CityArea { get; set; }

        [Display(Name ="Summary of property features")]
        public string SummaryFeature { get; set; }

        public List<PropertyFeatures> Features { get; set; }

        public List<Heating> Heating { get; set; }

        public ICollection<ListingImage> ImagesContent { get; set; }
      
        [NotMapped]
        public ListingImage Thumbnail => 
            ImagesContent.FirstOrDefault(i => i.IsDefaultImage && !i.IsArchived) ??
            ImagesContent.LastOrDefault(i => !i.IsArchived);


        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsActive { get; set; }

        public int? UserCreatorId { get; set; }

        public int? UserUpdatorId { get; set; }

        [ForeignKey("UserCreatorId")]
        [Display(Name = "Created By")]
        public  OperatingUser UserCreator { get; set; }
        
        [ForeignKey("UserUpdatorId")]
        [Display(Name = "Updated By")]
        public  OperatingUser UserUpdator { get; set; }

        public  List<Viewing> Viewings { get; set; }
    }
}