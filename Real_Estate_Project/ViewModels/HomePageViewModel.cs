using PagedList;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.ViewModels
{
    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
        }

        public IPagedList<Listing> Listings { get; set; }

        public Listing CurrentViewedListing { get; set; }

        public int CurrentPage { get; set; }

        public string CurrentSearch { get; set; }
    }
}