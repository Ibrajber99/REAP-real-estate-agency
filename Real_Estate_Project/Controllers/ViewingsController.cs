using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.Business_services.ViewingService;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Listing_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.ViewModels;

namespace Real_Estate_Project.Controllers
{
    public class ViewingsController : Controller
    {

        private const string NOT_AUTHORIZED_PATH = "RoleNotAuthorized";
        private SimpleErrorModel errorModel;
        private ViewingViewModel model;
        private IViewingRepo _viewingRepo;
        private IViewingService _viewingService;


        public ViewingsController
            (IViewingRepo viewingRepo,
            IViewingService viewingService,
            ViewingViewModel viewingVM,
            SimpleErrorModel errorM)
        {
            _viewingRepo = viewingRepo;
            _viewingService = viewingService;
            model = viewingVM;
            errorModel = errorM;

        }


        public async Task<ViewingViewModel> GetDefaultViewModelEntry()
        {
            var viewModel = new ViewingViewModel();
            var listings = _viewingService.GetListingsList();
            var customers = _viewingService.GetCustomersList();
            var agents = _viewingService.GetAgentsList();

            await Task.WhenAll(listings, customers, agents);

            viewModel.Listings = listings.Result;
            viewModel.Customers = customers.Result;
            viewModel.Agents = agents.Result;
            viewModel.DurationList = _viewingService.GetDurationSlots();

            return viewModel;
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Index()
        {
            try
            {
                var viewings = await _viewingRepo.GetViewingsByDate
                (DateTime.Now, DateTime.Now.AddMonths(3));

                if (viewings != null)
                    model.Viewings = viewings;
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> GetAllViewings()
        {
            try
            {
                var viewingsList =  await _viewingRepo.GetAll();

                if (viewingsList == null)
                    return HttpNotFound();

                model.Viewings = viewingsList;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError
                (string.Empty, ex.Message);
            }
            return View("Index",model);
        }


        [HttpPost]
        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> GetViewingsByDateRange
         (ViewingViewModel dateFilterInputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var viewingList =await _viewingRepo.GetViewingsByDate(
                        dateFilterInputModel.ViewingDateFilter.StartingDateRange,
                        dateFilterInputModel.ViewingDateFilter.EndingDateRange);

                    if (viewingList == null)
                        return HttpNotFound();

                    model.Viewings = viewingList;

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError
                    (string.Empty, ex.Message);
                }
                return View("Index", model);
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var viewingRes = await _viewingRepo.Get(id.Value);

                    if (viewingRes != null)
                    {
                        if (viewingRes.Listing.ImagesContent != null)
                        {
                            var listingImages = new List<ListingImage>
                                (viewingRes.Listing.ImagesContent);

                            viewingRes.Listing.ImagesContent = new List<ListingImage>();

                            listingImages.ForEach(i =>viewingRes.Listing.ImagesContent.Add(i));

                        }

                        model = _viewingService
                            .GetViewingViewModelFromModel(viewingRes);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return View(model);
            }
            return HttpNotFound();
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Create(int? listingId)
        {
            try
            {
                var currentLoggedUserId = User.Identity.GetUserId();

                var currentLoggedUserRes = await UserIdentityManager.GetUserById
                                           (currentLoggedUserId);

                if (!currentLoggedUserRes.registeredUser.IsVerified
                    && currentLoggedUserRes.registeredUser.RoleID == RoleNames.AGENT)
                {
                    errorModel.ErrorMessage = 
                        "Agent cannot access viewing resource.\n Agent is not verified";

                    return View(NOT_AUTHORIZED_PATH, errorModel);
                }


                model = await GetDefaultViewModelEntry();

                if (listingId.HasValue)
                {
                    model.ListingId = listingId.Value;

                    var listingInDb = await _viewingRepo.GetListingById(model.ListingId);

                    model.PickedListingPostalCode =
                        $@"{listingInDb.ListingAddress.PostalCode} { listingInDb.ListingAddress.StreetAddress} {listingInDb.ListingAddress.Municipality}";
                }
                else
                {
                    model.PickedListingPostalCode = "No Listing is Picked";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Create(ViewingViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if(model.ListingId == 0)
                    {
                        model = await GetDefaultViewModelEntry();

                        model.PickedListingPostalCode = "No Listing is Picked";
                        ModelState.AddModelError(string.Empty, "Please pick a Listing");

                        return View(model);
                    }

                    var modelState = ModelState;
                    var viewingToCreate = new Viewing();
                    viewingToCreate = await _viewingService
                            .GetViewingFromViewModel(model);

                    if (await _viewingService.IsViewingValid
                        (viewingToCreate,ModelState))
                    {
                        var currentLoggedUserId = User.Identity.GetUserId();
                        var currentLoggedUserRes = await UserIdentityManager
                            .GetUserById(currentLoggedUserId);

                        viewingToCreate.CreatedDate = DateTime.Now;
                        viewingToCreate.UpdatedDate = DateTime.Now;

                        viewingToCreate.UserCreatorId = 
                            currentLoggedUserRes.registeredUser.ID;

                        viewingToCreate.UserUpdatorId = 
                            currentLoggedUserRes.registeredUser.ID;

                        viewingToCreate.IsActive = true;

                        var result = await _viewingRepo.Create
                            (viewingToCreate);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        model = await GetDefaultViewModelEntry();
                        return View(model);
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Edit(int? listingId,int? id)
        {
            try
            {
                var currentLoggedUserId = User.Identity.GetUserId();

                var currentLoggedUserRes = await UserIdentityManager.GetUserById
                                           (currentLoggedUserId);

                if (!currentLoggedUserRes.registeredUser.IsVerified
                    && currentLoggedUserRes.registeredUser.RoleID == RoleNames.AGENT)
                {
                    errorModel.ErrorMessage =
                     "Agent cannot access viewing resource.\n Agent is not verified";

                    return View(NOT_AUTHORIZED_PATH, errorModel);
                }


                var viewingModel = await _viewingRepo.Get(id.Value);

                if (viewingModel == null)
                    return HttpNotFound();

                //If user Picked a different Listing on Edit
                if (listingId.HasValue)
                    viewingModel.Listing = await _viewingRepo.GetListingById(listingId.Value);

                model = _viewingService.GetViewingViewModelFromModel(viewingModel);

                var tmpModel = await GetDefaultViewModelEntry();

                model.Listings = tmpModel.Listings;
                model.Customers = tmpModel.Customers;
                model.Agents = tmpModel.Agents;
                model.DurationList = tmpModel.DurationList;
                model.PickedListingPostalCode = 
               $@"{ viewingModel.Listing.ListingAddress.PostalCode} { viewingModel.Listing.ListingAddress.StreetAddress} { viewingModel.Listing.ListingAddress.Municipality}";


            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Edit(ViewingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var modelState = ModelState;

                    var viewingToModify = await  _viewingService
                        .GetViewingFromViewModel(model);

                    viewingToModify.ID = model.ReadonlyViewingModel.ID;
                    viewingToModify.UserCreatorId = model.ReadonlyViewingModel.UserCreatorId;


                    if (await _viewingService.IsViewingValid
                     (viewingToModify, ModelState))
                    {
                        var currentLoggedUserId = User.Identity.GetUserId();

                        var currentLoggedUserRes = await UserIdentityManager
                                                .GetUserById(currentLoggedUserId);

                        viewingToModify.UpdatedDate = DateTime.Now;

                        viewingToModify.UserUpdatorId = 
                            currentLoggedUserRes.registeredUser.ID;

                        await _viewingRepo.Update(viewingToModify);

                        return RedirectToAction("Details",new { id = viewingToModify.ID });
                    }
                    else
                    {
                        var viewingModel = model.ReadonlyViewingModel;
                        model = await GetDefaultViewModelEntry();
                        model.ReadonlyViewingModel = viewingModel;

                        return View(model);
                    }
                }
                catch ( Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
           
            return View(model);
        }


        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                try
                {
                    var viewing = await _viewingRepo.Get(id.Value);

                    if (viewing == null)
                        return HttpNotFound();

                    model.ReadonlyViewingModel = viewing;

                    return View(model);
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


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.OFFICE_MANAGER + "," + RoleNames.ADMIN + "," + RoleNames.AGENT)]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var viewingModel = await _viewingRepo.Get(id);
                viewingModel.IsActive = false;

                var res = await _viewingRepo.Update(viewingModel);

                if (res != 1)
                {
                    ModelState.AddModelError(string.Empty,
                        "couldn't Archive Viewing");
                    model.ReadonlyViewingModel = await _viewingRepo.Get(id);

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.ReadonlyViewingModel = await _viewingRepo.Get(id);
                return View(model);
            }

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // db.Dispose();
                _viewingRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
