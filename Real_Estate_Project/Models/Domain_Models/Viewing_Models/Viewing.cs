using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.Models.Domain_Models.Viewing_Models
{
    public class Viewing
    {
        public Viewing()
        {
            ViewingHost = new OperatingUser();
            Listing = new Listing();
            Customer = new Customer();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Viewing Host")]
        public virtual OperatingUser ViewingHost { get; set; }


        [Display(Name = "Property to View")]
        public virtual Listing Listing { get; set; }

        [Required]
        [Display(Name = "Viewing Guest")]
        public virtual Customer Customer { get; set; }

        [Required]
        [Display(Name = "Starting on")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name ="Ending on")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated On")]
        public DateTime UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public int? UserCreatorId { get; set; }

        public int? UserUpdatorId { get; set; }

        [ForeignKey("UserCreatorId")]
        public  OperatingUser UserCreator { get; set; }

        [ForeignKey("UserUpdatorId")]
        public  OperatingUser UserUpdator { get; set; }
    }
}