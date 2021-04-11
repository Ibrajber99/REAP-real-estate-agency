using PagedList;
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Listing_Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Real_Estate_Project.ViewModels
{
    public class ListingViewModel
    {

        public List<OperatingUser> Agents { get; set; }

        public List<Customer> Customers { get; set; }

        public List<SelectListItem> HeatingListToSelect { get; set; }

        public List<SelectListItem> FeaturesListToSelect { get; set; }

        public List<SelectListItem> ExistingListingImages { get; set; }

        public Listing InputModel { get; set; }

        public IPagedList<Listing> Listings { get; set; }


        //List of HttpFileBase when user is editing and Creating a listing
        [Display(Name = "Select images to add (7 maximum)")]
        public List<HttpPostedFileBase> ListingImageFiles { get; set; }


        public ListingViewModel()
        {
            HeatingListToSelect = new List<SelectListItem>();
            FeaturesListToSelect = new List<SelectListItem>();
            ExistingListingImages = new List<SelectListItem>();

            Customers = new List<Customer>();
            Agents = new List<OperatingUser>();
            InputModel = new Listing();

            ListingImageFiles = new List<HttpPostedFileBase>();

        }
        
    }
}