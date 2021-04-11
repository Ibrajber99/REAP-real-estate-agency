using Real_Estate_Project.Models.Dashboard_Models;
using Real_Estate_Project.Models.Dashboard_Models.ChartClient;
using Real_Estate_Project.ViewModels.Dashboard_View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Project.Business_services
{
    public interface IDashboardService : IDisposable
    {
        Task<DashboardViewModel> GetDashboardViewModel();

        ChartBase GetUsersStatus();

        ChartBase GetUsersTypeCount();

        ChartBase GetAgentsVerificationStatus();

        ChartBase GetCustomersStatus();

        ChartBase GetCustomersListingsCount();

        ChartBase GetCustomersViewingsCount();

        ChartBase GetListingsStatus();

        ChartBase GetPastAndCurrentViewingsCount();

        //Charts Client response list
         Task<List<ClientResponseModel>> GetUsersChartsResponse();

         Task<List<ClientResponseModel>> GetCustomersChartsResponse();

         Task<List<ClientResponseModel>> GetListingsChartsResponse();

         Task<List<ClientResponseModel>>GetViewingsChartsResponse();

    }
}
