using PagedList.EntityFramework;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models.Listing_Models;
using Real_Estate_Project.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Real_Estate_Project.Controllers
{
    public class HomeController : Controller
    {
        private IListingRepo _listingRepo;
        private HomePageViewModel model;

        public async Task<ActionResult> DisplayFile(int id)
        {
            try
            {
                var imageToDisplay = await _listingRepo.GetListingThumbnail(id);

                if (imageToDisplay == null)
                    return File(new byte[0], string.Empty);


                return File(imageToDisplay.ThumbnailContent,
                    imageToDisplay.ContentType);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return File(new byte[0], string.Empty);
            }

        }

        public async Task<ActionResult> DisplayFullImage(int id)
        {
            try
            {
                var imageToDisplay = await _listingRepo.GetListingImage(id);

                if (imageToDisplay == null)
                    return File(new byte[0], string.Empty);


                return File(imageToDisplay.Content,
                    imageToDisplay.ContentType);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return File(new byte[0], string.Empty);
            }
        }

        public HomeController
            (IListingRepo listingRepo,
            HomePageViewModel homePageVM)
        {
            _listingRepo = listingRepo;
            model = homePageVM;
        }

        public async Task<ActionResult> Index
           (int? requestedListingId,int page =1,
            string CurrentSearch = null)
        {
            try
            {
                Listing requestedListing;

                var listings = await
                     _listingRepo.GetIqueryableListings(CurrentSearch)
                     .ToPagedListAsync(page, 6);


                if (listings == null)
                    return HttpNotFound();

                model.Listings = listings;


                if (requestedListingId.HasValue)
                {
                    requestedListing = await 
                        _listingRepo.Get(requestedListingId.Value);

                    if (requestedListing == null)
                        return HttpNotFound();

                    model.CurrentViewedListing = requestedListing;
                }

                model.CurrentPage = page;
                model.CurrentSearch = CurrentSearch;

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError
                    (string.Empty, ex.Message);

                return View(model);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}