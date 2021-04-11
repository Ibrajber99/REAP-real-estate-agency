using Real_Estate_Project.Models.Domain_Models.Listing_Models;
using Real_Estate_Project.Models.Listing_Models;
using Real_Estate_Project.ViewModels;
using Real_Estate_Project.ViewModels.Listing_ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;



namespace Real_Estate_Project
{
    public static class ListingViewHelperUtilites
    {
        public static List<SelectListItem> GetHeatingTypesToListItems
            (List<Heating> heatingList)
        {
            List<SelectListItem> listItemRes = new List<SelectListItem>();

            foreach (var heating in heatingList)
            {
                var selectItem = new SelectListItem();
                selectItem.Text = heating.HeatingType;
                selectItem.Value = heating.ID.ToString();
                selectItem.Selected = false;
                listItemRes.Add(selectItem);
            }

            return listItemRes;
        }

        public static List<SelectListItem> GetPropertyFeaturesToListItems
            (List<PropertyFeatures> propFeaturesList)
        {
            List<SelectListItem> listItemRes = new List<SelectListItem>();

            foreach (var feature in propFeaturesList)
            {
                var selectItem = new SelectListItem();
                selectItem.Text = feature.FeatureName;
                selectItem.Value = feature.ID.ToString();
                selectItem.Selected = false;
                listItemRes.Add(selectItem);
            }

            return listItemRes;
        }

        public static List<SelectListItem> SetImagesToListItems
            (List<ListingImage> imagesList)
        {
            List<SelectListItem> listItemRes = new List<SelectListItem>();

            foreach (var image in imagesList)
            {
                var selectItem = new SelectListItem();
                selectItem.Text = image.FileName;
                selectItem.Value = image.ID.ToString();
                selectItem.Selected = false;
                listItemRes.Add(selectItem);
            }

            return listItemRes;
        }

        public static void SetHeatingTypesToListItems
            (this List<SelectListItem> selectList, List<Heating> heatingList)
        {

            foreach (var heatingInModel in heatingList)
            {
                foreach (var heatingInSelectList in selectList)
                {
                    if (heatingInSelectList.Value == heatingInModel.ID.ToString())
                    {
                        heatingInSelectList.Selected = true;
                        break;
                    }
                }
            }
        }

        public static Listing MapHeatingAndFeaturesToListingModel
            (ListingViewModel model)
        {

            model.InputModel.Heating = GetHeatingListFromListItems(model.HeatingListToSelect);
            model.InputModel.Features = GetFeaturesFromListItems(model.FeaturesListToSelect);

            return model.InputModel;
        }

        public static List<Heating>GetHeatingListFromListItems
            (List<SelectListItem> heatingList)
        {
            var heatingModelList = new List<Heating>();

            foreach (var item in heatingList)
            {
                if (item.Selected)
                {
                    var heating = new Heating
                    {
                        ID = Convert.ToInt32(item.Value),
                        HeatingType = item.Text
                    };
                    heatingModelList.Add(heating);
                }
            }

            return heatingModelList;
        }

        public static List<PropertyFeatures>GetFeaturesFromListItems
            (List<SelectListItem> featuresList)
        {
            var featuresModelList = new List<PropertyFeatures>();

            foreach (var item in featuresList)
            {
                if (item.Selected)
                {
                    var heating = new PropertyFeatures
                    {
                        ID = Convert.ToInt32(item.Value),
                        FeatureName = item.Text
                    };
                    featuresModelList.Add(heating);
                }
            }

            return featuresModelList;
        }


        public static void SetFeaturesTypesToListItems
            (this List<SelectListItem> selectList ,
            List<PropertyFeatures> propFeaturesList)
        {
            
            foreach (var featureInModel in propFeaturesList)
            {
                foreach (var featureInSelectList in selectList)
                {
                    if (featureInSelectList.Value == featureInModel.ID.ToString())
                    {
                        featureInSelectList.Selected = true;
                        break;
                    }
                }
            }
        }

        public static Dictionary<string, Pair> GetListingPriceRange()
        {
            var priceRangeList = new Dictionary<string,Pair>() {
                {"100$ - 100,000$" ,new Pair(100, 100000) }, 
                {"101,000$ - 300,000$",new Pair(101000, 300000) },
                {"301,000$ - 500,000$",new Pair(301000, 500000) }, 
                {"501,000$ - 10,000,000$",new Pair(501000, 1000000) }
            };

            return priceRangeList;
        }

        public static Pair GetPairPriceRange(string priceString)
        {
            var pricePair = new Pair(0d, 0d);

            if (!string.IsNullOrEmpty(priceString))
            {
                var priceRangeList = GetListingPriceRange();
                var kvpFound = priceRangeList.FirstOrDefault(i => i.Key.Equals(priceString));

                if (kvpFound.Value != null && !string.IsNullOrEmpty(kvpFound.Key))
                {
                    pricePair.First = kvpFound.Value.First;
                    pricePair.Second = kvpFound.Value.Second;
                }
            }
            return pricePair;
        }

        public static SearchBoxViewModel GetSearchBoxModel(int? numOfBeds,
         int? numOfBaths, int? numOfStories,
         string municipality = null, string priceRangeString = null)
        {
            var model = new SearchBoxViewModel
            {
                Municipality = municipality,
                PriceRangeString = priceRangeString,
                NumOfBeds = numOfBeds,
                NumOfBaths = numOfBaths,
                NumOfStories = numOfStories
            };

            model.PriceRange = GetPairPriceRange(model.PriceRangeString);

            return model;
        }

        public static SearchListingViewModel GetDefaultSearchParams
            (string orderBy = null)
        {
            var model = new SearchListingViewModel();
            model.PostalCodeParam = orderBy == "PostalCode" ? "PostalCode_Desc" : "PostalCode";
            model.PriceParam = orderBy == "Price" ? "Price_Desc" : "Price";
            model.SquareFootageParam = orderBy == "SquareFootage" ? "SquareFootage_Desc" : "SquareFootage";
            model.NumOfBedsParam = orderBy == "NumOfBeds" ? "NumOfBeds_Desc" : "NumOfBeds";
            model.NumOfBathsParam = orderBy == "NumOfBaths" ? "NumOfBaths_Desc" : "NumOfBaths";
            model.NumOfStoriesParam = orderBy == "NumOfStories" ? "NumOfStories_Desc" : "NumOfStories";

            model.PriceRangeList = GetListingPriceRange();

            return model;
        }
    }
}