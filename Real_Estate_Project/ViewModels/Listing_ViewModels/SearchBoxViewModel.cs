using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Real_Estate_Project.ViewModels.Listing_ViewModels
{
    public class SearchBoxViewModel
    {
        public SearchBoxViewModel()
        {
            PriceRange = new Pair(0,0);
        }

        [Display(Name = "Municipality")]
        public string Municipality { get; set; }

        [Display(Name ="Price range")]
        public Pair PriceRange { get; set; }

        public string PriceRangeString { get; set; }

        [Display(Name = "Number of beds")]
        [Range(1,30)]
        public int? NumOfBeds { get; set; }

        [Display(Name = "Number of baths")]
        [Range(1, 30)]
        public int? NumOfBaths { get; set; }

        [Display(Name = "Number of stories")]
        [Range(1, 30)]
        public int? NumOfStories { get; set; }

    }
}