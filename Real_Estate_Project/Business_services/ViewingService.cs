using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;
using Real_Estate_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Real_Estate_Project.Viewing_Services
{
    public static class ViewingService
    {
        private const int TRAVEL_TIME = 15;

        public static async Task<List<OperatingUser>> GetAgentsList(IViewingRepo repo)
        {
            var usersList = await repo.GetAgents();
            if (usersList == null)
                return new List<OperatingUser>();
            else
                return usersList;
        }

        public static async Task<List<Customer>> GetCustomersList(IViewingRepo repo)
        {
            var customersList = await repo.GetCustomers();
            if (customersList == null)
                return new List<Customer>();
            else
                return customersList;
        }

        public static async Task<List<Listing>> GetListingsList(IViewingRepo repo)
        {
            var listingsList = await repo.GetListings();
            if (listingsList == null)
                return new List<Listing>();
            else
                return listingsList;
        }

        public static async Task<bool> IsViewingValid
            (IViewingRepo viewingRepo, Viewing viewing,
             ModelStateDictionary modelState)
        {
            if (viewing.StartDate.DayOfWeek > DayOfWeek.Sunday
               && viewing.StartDate.DayOfWeek < DayOfWeek.Saturday)
            {
                if (viewing.StartDate.Date.Date >= DateTime.Now.Date)
                {
                    var agentAvailableRes = isAgentAvailable(viewingRepo, viewing);
                    var customerAvailableRes = isCustomerAvailable(viewingRepo, viewing);
                    var listingAvailableRes = isListingAvailable(viewingRepo, viewing);

                    await Task.WhenAll(agentAvailableRes, customerAvailableRes, listingAvailableRes);

                    if (!agentAvailableRes.Result)
                        modelState.AddModelError(string.Empty, "The Agent selected is not avialble on this time");


                    if (!customerAvailableRes.Result)
                        modelState.AddModelError(string.Empty, "The Customer selected is not avialble on this time");


                    if (!listingAvailableRes.Result)
                        modelState.AddModelError(string.Empty, "The Listing Poperty in not available at that time");


                    return modelState.IsValid ? true : false;
                }
                else
                {
                    modelState.AddModelError(string.Empty, "The Viewing cannot be in the past");
                    return false;
                }
            }
            modelState.AddModelError(string.Empty, $"Viewing is not available on {viewing.StartDate.DayOfWeek}s");
            return false;
        }

        public static async Task<bool> isAgentAvailable(IViewingRepo viewingRepo, Viewing viewing)
        {
            //Getting results for the host on that specific date (MM/DD/YYY)
            var result = await viewingRepo.GetAll();

            result = result.Where(view => view.ViewingHost.ID == viewing.ViewingHost.ID
            && view.StartDate.Date == viewing.StartDate.Date
            && view.ID != viewing.ID
            && view.IsActive == true).ToList();

            if (result.Count > 0)
            {
                return ValidateViewingAvailability(result, viewing);
            }
            else
                return true;
        }

        public static async Task<bool> isCustomerAvailable(IViewingRepo viewingRepo, Viewing viewing)
        {
            var result = await viewingRepo.GetAll();

            result = result.Where(view => view.Customer.ID == viewing.Customer.ID && 
            view.StartDate.Date == viewing.StartDate.Date
            && view.ID != viewing.ID
            && view.IsActive == true).ToList();

            if (result.Count > 0)
            {
                return ValidateViewingAvailability(result, viewing);
            }
            else
                return true;
        }

        public static async Task<bool> isListingAvailable(IViewingRepo viewingRepo, Viewing viewing)
        {
            var result = await viewingRepo.GetAll();

            result = result.Where(view => view.Listing.ID == viewing.Listing.ID
                && view.StartDate.Date == viewing.StartDate.Date
                && view.ID != viewing.ID
                && view.IsActive == true).ToList();

            if (result.Count > 0)
            {
                return ValidateViewingAvailability(result, viewing);
            }
            else
                return true;
        }

        public static bool ValidateViewingAvailability(List<Viewing> viewingsInDb,
            Viewing viewingToCheckFor)
        {

            foreach (var currDate in viewingsInDb)
            {
                var isAvailable = (viewingToCheckFor.StartDate < currDate.StartDate && 
                                    viewingToCheckFor.EndDate < currDate.StartDate)
                                                              ||
                                  (viewingToCheckFor.EndDate > currDate.EndDate && 
                                  viewingToCheckFor.StartDate > currDate.EndDate);
                if (!isAvailable)
                    return false;
            }

            return true;
        }

        public static async Task<Viewing> GetViewingFromViewModel
        (IViewingRepo repo, ViewingViewModel viewModel)
        {

            var agent = repo.GetAgentById(viewModel.AgentId);
            var customer = repo.GetCustomerById(viewModel.CustomerID);
            var listing = repo.GetListingById(viewModel.ListingId);

            await Task.WhenAll(agent, customer, listing);

            Viewing res = new Viewing();

            res.ViewingHost = agent.Result;
            res.Customer = customer.Result;
            res.Listing = listing.Result;
            res.StartDate = viewModel.ViewingStart.AddMinutes(TRAVEL_TIME);
            res.EndDate = res.StartDate.AddMinutes(viewModel.ViewingDuration).
                            AddMinutes(TRAVEL_TIME);

            //If null means there is a new entry
            res.IsActive = viewModel.ReadonlyViewingModel == null ?
                true : viewModel.ReadonlyViewingModel.IsActive;

            return res;
        }

        public static ViewingViewModel GetViewingViewModelFromModel(Viewing view)
        {
            ViewingViewModel viewmodel = new ViewingViewModel();

            viewmodel.AgentId = view.ViewingHost.ID;
            viewmodel.ListingId = view.Listing.ID;
            viewmodel.CustomerID = view.Customer.ID;
            viewmodel.ViewingStart = view.StartDate.AddMinutes(-TRAVEL_TIME);

            viewmodel.ViewingDuration = (int)(view.EndDate.TimeOfDay.TotalMinutes 
                                        - view.StartDate.TimeOfDay.TotalMinutes) - (TRAVEL_TIME);


            viewmodel.ReadonlyViewingModel = view;

            return viewmodel;
        }

        public static List<int> GetDurationSlots()
        {
            var durationList = new List<int>();

            for(int i =15; i<= 120; i += 15)
            {
                durationList.Add(i);
            }

            return durationList;
        }
    }
}