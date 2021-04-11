using PagedList;
using Real_Estate_Project.Models.Listing_Models;
using Real_Estate_Project.ViewModels.Listing_ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.UI;

namespace Real_Estate_Project.ViewModels
{
    public class SearchListingViewModel
    {
        public SearchListingViewModel()
        {
            SearchModel = new SearchBoxViewModel();
        }


        //Only for When Editing
        public int? ViewingID { get; set; }

        public IPagedList<Listing> Listings { get; set; }

        public Listing DisplayOnlyModel { get; set; }


        //Order by Params
        public string PostalCodeParam { get; set; }

        public string PriceParam { get; set; }

        public string SquareFootageParam { get; set; }

        public string NumOfBedsParam { get; set; }

        public string NumOfBathsParam { get; set; }

        public string NumOfStoriesParam { get; set; }

        public string CurrentSort { get; set; }

        //Searching Params
        public string CurrentMunicipalitySearch { get; set; }

        [Display(Name ="Filter By Heating Type")]
        public List<SelectListItem> heatingTypesSearch { get; set; }


        [Display(Name ="Filter By Feature")]
        public List<SelectListItem> propertyFeaturesSearch { get; set; }

        public SearchBoxViewModel SearchModel { get; set; }

        public Dictionary<string, Pair> PriceRangeList { get; set; }
    }
}