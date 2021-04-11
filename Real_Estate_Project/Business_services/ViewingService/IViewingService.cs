using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;
using Real_Estate_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Real_Estate_Project.Business_services.ViewingService
{
    public interface IViewingService
    {
        Task<List<OperatingUser>> GetAgentsList();

        Task<List<Customer>> GetCustomersList();

        Task<List<Listing>> GetListingsList();

        Task<bool> IsViewingValid
            (Viewing viewing,
             ModelStateDictionary modelState);

        Task<bool> isAgentAvailable(Viewing viewing);

        Task<bool> isCustomerAvailable(Viewing viewing);

        Task<bool> isListingAvailable(Viewing viewing);

        bool ValidateViewingAvailability(List<Viewing> viewingsInDb,
            Viewing viewingToCheckFor);

        Task<Viewing> GetViewingFromViewModel
        (ViewingViewModel viewModel);

        ViewingViewModel GetViewingViewModelFromModel(Viewing view);

        List<int> GetDurationSlots();
    }
}
