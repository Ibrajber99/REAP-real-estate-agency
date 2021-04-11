using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.EntityFramework;
using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models.Listing_Models;
using Real_Estate_Project.View_Uitlity_Model_Helpers;
using Real_Estate_Project.ViewModels;
using Real_Estate_Project.ViewModels.Listing_ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Real_Estate_Project.Controllers
{

    public class ListingOperationsController : Controller
    {
        private static string DEFAULT_IMAGE = 
            "https://www.flaticon.com/premium-icon/icons/svg/2243/2243783.svg";

        private ICustomerRepo _customerRepo;
        private IOperatingUserRepo _userRepo;
        private IListingRepo _listingRepo;
        private IListingImageHelper _imageHelper;
        private ListingViewModel model;

        public ListingOperationsController
            (IOperatingUserRepo userRepo, ICustomerRepo customerRepo
            , IListingRepo listingRepo, ListingViewModel listingVM, IListingImageHelper imageHelper)
        {
            _userRepo = userRepo;
            _customerRepo = customerRepo;
            _listingRepo = listingRepo;
            model = listingVM;
            _imageHelper = imageHelper;
        }


        public async Task<ActionResult> DisplayFile(int id)
        {
            try
            { 
                var imageToDisplay = await _listingRepo.GetListingThumbnail(id);

                if (imageToDisplay == null)
                    return Content(DEFAULT_IMAGE);


                return File(imageToDisplay.ThumbnailContent, imageToDisplay.ContentType);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return File(new byte[0],string.Empty);
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

        public async Task<ListingViewModel> GetDefaultViewModelEntry()
        {
            var viewModel = new ListingViewModel();

            var heatingDbList = _listingRepo.GetAllHeating();
            var propFeaturesDbList = _listingRepo.GetAllFeatures();
            var customersDbList = _customerRepo.GetAll();
            var agentsDbList = _userRepo.GetAllByRole(RoleNames.AGENT);


            await Task.WhenAll(heatingDbList, propFeaturesDbList,
                                customersDbList, agentsDbList);


            viewModel.HeatingListToSelect = ListingViewHelperUtilites.
            GetHeatingTypesToListItems(heatingDbList.Result);

            viewModel.FeaturesListToSelect = ListingViewHelperUtilites.
            GetPropertyFeaturesToListItems(propFeaturesDbList.Result);

            viewModel.Agents = agentsDbList.Result;

            viewModel.Customers = customersDbList.Result;

            return viewModel;
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Index(int page =1)
        {
            try
            {
                var listings = _listingRepo.GetIqueryableListings();

                if (listings != null)
                    model.Listings = await listings.ToPagedListAsync(page,8);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var listing = await _listingRepo.Get(id.Value);

                    if (listing == null)
                        return HttpNotFound();

                    model.InputModel = listing;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Create()
        {
            try
            {
                model = await GetDefaultViewModelEntry();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        [HttpPost]
        public async Task<ActionResult> Create(ListingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var modelToInsert = model.InputModel;

                    var currentLoggedUserId = User.Identity.GetUserId();
                    var currentLoggedUserRes = await UserIdentityManager.GetUserById(currentLoggedUserId);

                    modelToInsert = ListingViewHelperUtilites
                        .MapHeatingAndFeaturesToListingModel(model);


                    modelToInsert.DateCreated = DateTime.Now;
                    modelToInsert.DateUpdated = DateTime.Now;
                    modelToInsert.UserCreatorId = currentLoggedUserRes.registeredUser.ID;
                    modelToInsert.UserUpdatorId = currentLoggedUserRes.registeredUser.ID;
                    modelToInsert.IsActive = true;

                    
                    if(!_imageHelper.IsPostedFilesListEmpty(model.ListingImageFiles))
                    {
                        if (_imageHelper.HasImageListExceededSizeLimit(model.ListingImageFiles))
                        {
                            ModelState.AddModelError
                                (string.Empty,
                                "File upload limit exceeded (more than 7 files)");

                            model = await GetDefaultViewModelEntry();
                            model.InputModel = modelToInsert;

                            return View(model);
                        }

                        if (_imageHelper.isValidFileUpload(model.ListingImageFiles))
                        {
                            var imagesList = model.ListingImageFiles;
                            _imageHelper.SetModelImages
                                (modelToInsert, imagesList);
                        }
                        else
                        {
                            ModelState.AddModelError
                                (string.Empty,
                                "One or more File uploads was not valid");

                            model = await GetDefaultViewModelEntry();
                            model.InputModel = modelToInsert;

                            return View(model);

                        }
                    }

                    int res = await _listingRepo.Create(modelToInsert);

                    return RedirectToAction("Details",new { id = modelToInsert.ID});
                }
                else
                {
                    var tmpModel = model.InputModel;
                    model = await GetDefaultViewModelEntry();
                    model.InputModel = tmpModel;

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model = await GetDefaultViewModelEntry();

                return View(model);
            }
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id.HasValue)
                {
                    var listingRes = await _listingRepo.Get(id.Value);

                    if (listingRes != null)
                    {
                        model = await GetDefaultViewModelEntry();

                        model.ExistingListingImages = 
                            ListingViewHelperUtilites.SetImagesToListItems
                            (listingRes.ImagesContent.ToList());


                        model.FeaturesListToSelect
                            .SetFeaturesTypesToListItems(listingRes.Features);

                        model.HeatingListToSelect
                            .SetHeatingTypesToListItems(listingRes.Heating);

                        model.InputModel = listingRes;

                        return View(model);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    return HttpNotFound();
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        [HttpPost]
        public async Task<ActionResult> Edit(ListingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listingFromDb = await _listingRepo.Get(model.InputModel.ID);

                    model.InputModel.ImagesContent = listingFromDb.ImagesContent;

                    var modelToUpdate = model.InputModel;

                    if (!_imageHelper.IsPostedFilesListEmpty(model.ListingImageFiles))
                    {

                        if (_imageHelper.HasImageListExceededSizeLimit
                            (model.ListingImageFiles))
                        {
                            ModelState.AddModelError
                                (string.Empty,
                                "File upload limit exceeded (more than 7 files)");

                            model = await GetDefaultViewModelEntry();
                            model.InputModel = modelToUpdate;

                            model.ExistingListingImages =
                            ListingViewHelperUtilites.SetImagesToListItems
                            (modelToUpdate.ImagesContent.ToList());

                            return View(model);
                        }

                        if (_imageHelper.isValidFileUpload(model.ListingImageFiles))
                        {
                            var imagesList = model.ListingImageFiles;
                            _imageHelper.SetModelImages
                                (modelToUpdate, imagesList);
                        }
                        else
                        {
                            ModelState.AddModelError
                                (string.Empty,
                                "One or more File uploads was not valid");

                            model = await GetDefaultViewModelEntry();
                            model.InputModel = modelToUpdate;

                            return View(model);

                        }
                    }

                    _imageHelper.ArchiveCheckedImages
                    (model.ExistingListingImages, modelToUpdate.ImagesContent.ToList());


                    if (modelToUpdate.ImagesContent
                        .Count(i => !i.IsArchived) > 7)
                    {
                        ModelState.AddModelError
                        (string.Empty, 
                        "A listing can't have more than 7 Images");

                        model = await GetDefaultViewModelEntry();
                        model.InputModel = modelToUpdate;

                        var imagesContentList =
                            modelToUpdate.ImagesContent.
                            Where(i => i.ID > 0).ToList();


                        model.ExistingListingImages =
                            ListingViewHelperUtilites.SetImagesToListItems
                            (imagesContentList);

                        return View(model);
                    }

                    
                    var currentLoggedUserId = User.Identity.GetUserId();

                    var currentLoggedUserRes = await UserIdentityManager
                        .GetUserById(currentLoggedUserId);

                    var tmpModel = ListingViewHelperUtilites
                        .MapHeatingAndFeaturesToListingModel(model);


                    modelToUpdate.Heating = tmpModel.Heating;
                    modelToUpdate.Features = tmpModel.Features;
                    modelToUpdate.DateUpdated = DateTime.Now;
                    modelToUpdate.UserUpdatorId = currentLoggedUserRes.registeredUser.ID;


                    await _listingRepo.Update(modelToUpdate);

                    model.InputModel = await _listingRepo.Get(modelToUpdate.ID);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var inputModel = model.InputModel;
                model = await GetDefaultViewModelEntry();
                model.InputModel = inputModel;


                model.FeaturesListToSelect.
                    SetFeaturesTypesToListItems(model.InputModel.Features);

                model.HeatingListToSelect.
                    SetHeatingTypesToListItems(model.InputModel.Heating);

                return View(model);
            }

            return View("Details", model);
        }


        [HttpGet]
        public async Task<ActionResult> SetDefaultImage(int? imageId, int? listingId)
        {
            try
            {
                if (!imageId.HasValue)
                {
                    return HttpNotFound();
                }

                if (!listingId.HasValue)
                {
                    return HttpNotFound();
                }

                var listingFound = 
                    _listingRepo.GetIqueryableListings()
                    .Include(i => i.ImagesContent)
                    .FirstOrDefault(i => i.ID == listingId);

                if (listingFound == null)
                    return HttpNotFound();

                var listingToUpdate = listingFound;

                _imageHelper.SetListingDefaultImage
                    (imageId.Value, listingToUpdate);

                await _listingRepo.Update(listingToUpdate);

                return RedirectToAction("Details", 
                    new { id = listingToUpdate.ID });
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var listing = await _listingRepo.Get(id.Value);

                    if (listing != null)
                        model.InputModel = listing;

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return View(model);
            }

            return HttpNotFound();
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var res = await _listingRepo.Delete(id);
                if(res != 1)
                {
                    ModelState.AddModelError(string.Empty,
                        "couldn't Archive Listing");

                    model.InputModel = await _listingRepo.Get(id);

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.InputModel = await _listingRepo.Get(id);

                return View(model);
            }

            return RedirectToAction("Index");
        }


        #region Filtering and searching related

        [HttpGet]
        public async Task<ActionResult> SearchListing
        (int? ViewingId, int? page, 
         int? listingRequestedDetails,int? numOfBeds,
         int? numOfBaths,int? numOfStories,
         string municipality = null, string priceRangeString = null,
         string orderBy = null)
        {

            var model = ListingViewHelperUtilites.GetDefaultSearchParams(orderBy);

            model.SearchModel = ListingViewHelperUtilites.GetSearchBoxModel
                                (numOfBeds, numOfBaths,
                                numOfStories, municipality, priceRangeString);

            var sortedList = await GetsortedListing
            (model.SearchModel ?? new SearchBoxViewModel(), orderBy, page);


            if (ViewingId.HasValue)
            {
                model.ViewingID = ViewingId;//We know it is in editing viewing
            }

            model.CurrentSort = orderBy;
            model.Listings = sortedList;


            if (listingRequestedDetails.HasValue)
            {
                model.DisplayOnlyModel = await _listingRepo
                    .Get(listingRequestedDetails.Value);
            }


            if (model.DisplayOnlyModel == null)
                model.DisplayOnlyModel = new Listing();

            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> SearchListing
            (int? ViewingId,int? page,int? listingRequestedDetails,
            SearchBoxViewModel searchModel,string orderBy = null)
        {

            var model = ListingViewHelperUtilites.GetDefaultSearchParams(orderBy);

            if (searchModel != null)
            {
                searchModel.PriceRange = ListingViewHelperUtilites
                    .GetPairPriceRange(searchModel.PriceRangeString);
            }


            var sortedList = await GetsortedListing
                (searchModel ?? new SearchBoxViewModel(), orderBy, page);


            if (ViewingId.HasValue)
                model.ViewingID = ViewingId;//We know it is in edit mode


            model.CurrentSort = orderBy;
            model.Listings = sortedList;
            model.SearchModel = searchModel;


            if (listingRequestedDetails.HasValue)
            {
                model.DisplayOnlyModel = await _listingRepo
                    .Get(listingRequestedDetails.Value);
            }

            
            if (model.DisplayOnlyModel == null)
                model.DisplayOnlyModel = new Listing();

            return View(model);
        }

        public async Task<IPagedList<Listing>> GetsortedListing(
            SearchBoxViewModel searchModel,
        string orderBy = null,int? page = 1)
        {
            var minPrice =  Convert.ToDecimal(searchModel.PriceRange.First);
            var maxPrice = Convert.ToDecimal(searchModel.PriceRange.Second);


            var query = _listingRepo.GetIqueryableListings(
                searchModel.Municipality,
                minPrice,maxPrice,searchModel.NumOfBeds ?? 0,
                searchModel.NumOfBaths ?? 0,searchModel.NumOfStories ?? 0);


            switch (orderBy)
            {
                case "PostalCode":
                    query = query.OrderBy(x => x.ListingAddress.PostalCode);
                    break;
                case "PostalCode_Desc":
                    query = query.OrderByDescending(x => x.ListingAddress.PostalCode);
                    break;
                case "Price":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "Price_Desc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case "SquareFootage":
                    query = query.OrderBy(x => x.SquareFootage);
                    break;
                case "SquareFootage_Desc":
                    query = query.OrderByDescending(x => x.SquareFootage);
                    break;
                case "NumOfBeds":
                    query = query.OrderBy(x => x.NumOfBeds);
                    break;
                case "NumOfBeds_Desc":
                    query = query.OrderByDescending(x => x.NumOfBeds);
                    break;
                case "NumOfBaths":
                    query = query.OrderBy(x => x.NumOfBaths);
                    break;
                case "NumOfBaths_Desc":
                    query = query.OrderByDescending(x => x.NumOfBaths);
                    break;
                case "NumOfStories":
                    query = query.OrderBy(x => x.NumOfStories);
                    break;
                case "NumOfStories_Desc":
                    query = query.OrderByDescending(x => x.NumOfStories);
                    break;
                default:
                    query = query.OrderBy(l => l.ID);
                    break;
            }

            int pageNumber = page ?? 1;

            return await query.ToPagedListAsync(pageNumber,10);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _userRepo.Dispose();
                _customerRepo.Dispose();
                _listingRepo.Dispose();
            }
        }
    }
}
