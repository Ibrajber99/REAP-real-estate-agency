using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;
using Real_Estate_Project.ViewModels.Viewing_View_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.ViewModels
{
    public class ViewingViewModel
    {
        public ViewingViewModel()
        {
            Listings = new List<Listing>();
            Agents = new List<OperatingUser>();
            Customers = new List<Customer>();
            Viewings = new List<Viewing>();
            DurationList = new List<int>();
            ViewingStart = DateTime.Now;
        }

        [Required]
        [Display(Name="Hosting Agent")]
        public int AgentId { get; set; }
        
        [Required]
        [Display(Name ="Viewing Customer")]
        public int CustomerID { get; set; }

        [Required]
        [Display(Name ="Property")]
        public int ListingId { get; set; }

        public string PickedListingPostalCode { get; set; }

        public int CreatedById { get; set; }

        public int UpdatedById { get; set; }

        [Required]
        [Display(Name ="Viewing Date and Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true,
        DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime ViewingStart { get; set; }


        [Display(Name ="Duration In minutes")]
        public int ViewingDuration { get; set; }

        public List<int> DurationList { get; set; }

        public ViewingDateFilter ViewingDateFilter { get; set; }

        public List<Viewing> Viewings { get; set; }

        public List<Listing> Listings { get; set; }

        public List<OperatingUser> Agents { get; set; }

        public List<Customer> Customers { get; set; }

        public Viewing ReadonlyViewingModel { get; set; }

    }
}