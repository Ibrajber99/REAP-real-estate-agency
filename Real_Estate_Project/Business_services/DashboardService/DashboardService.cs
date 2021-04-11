using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models.Dashboard_Models;
using Real_Estate_Project.Models.Dashboard_Models.ChartClient;
using Real_Estate_Project.ViewModels.Dashboard_View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Real_Estate_Project.Business_services
{
    public class DashboardService : IDashboardService
    {
        IBaseClient<ChartBase> _chartClient;
        IDashboardRepo _dashboardRepo;

        public DashboardService(IBaseClient<ChartBase> chartClient,
            IDashboardRepo dashboardRepo)
        {
            _chartClient = chartClient;
            _dashboardRepo = dashboardRepo;
        }


        public async Task<DashboardViewModel> GetDashboardViewModel()
        {
            var model = new DashboardViewModel();

            var usersChartsTask = GetUsersChartsResponse();
            var customersChartsTask = GetCustomersChartsResponse();
            var listingsChartsTask = GetListingsChartsResponse();
            var viewingsChartsTask = GetViewingsChartsResponse();

            await Task.WhenAll(usersChartsTask, customersChartsTask,
                listingsChartsTask, viewingsChartsTask);

            model.UsersCharts.AddRange(usersChartsTask.Result);
            model.CustomersCharts.AddRange(customersChartsTask.Result);
            model.ListingsCharts.AddRange(listingsChartsTask.Result);
            model.ViewingsCharts.AddRange(viewingsChartsTask.Result);

            return model;
        }


        #region Charts Methods
        public ChartBase GetUsersStatus()
        {
            var query = _dashboardRepo.GetIqueryableUsers();

            var activeUsersCount = query.Count(i => i.IsActive);
            var archivedUsersCount = query.Count(i => !i.IsActive);

            ChartBase chart = new DoughnutChart();
            chart.Data = new DataModel
            {
                Labels = new List<string> { "Active users", "Archived users" },
                Datasets = new List<DataSetModel>{ new DataSetModel
                {Data = new List<int> { activeUsersCount, archivedUsersCount } } }
            };

            return chart;
        }

        public ChartBase GetUsersTypeCount()
        {
            var query = _dashboardRepo.GetIqueryableUsers();

            var officeManagers = query.Count(i => i.RoleID == RoleNames.OFFICE_MANAGER);
            var admins = query.Count(i => i.RoleID == RoleNames.ADMIN);
            var agents = query.Count(i => i.RoleID == RoleNames.AGENT);

            ChartBase chart = new DoughnutChart();
            chart.Data = new DataModel
            {
                Labels = new List<string> { "Office manager", "Admin", "Agent" },
                Datasets = new List<DataSetModel>{ new DataSetModel
                {Data = new List<int> { officeManagers, admins,agents } } }
            };

            return chart;
        }

        public ChartBase GetAgentsVerificationStatus()
        {
            var query = _dashboardRepo.GetIqueryableUsers();

            var verifiedAgents = query.Count
                (i => i.RoleID == RoleNames.AGENT && i.IsVerified);

            var pendingAgents = query.Count
                (i => i.RoleID == RoleNames.AGENT && !i.IsVerified);


            ChartBase chart = new DoughnutChart();
            chart.Data = new DataModel
            {
                Labels = new List<string> { "Verified agents", "Pending agents" },
                Datasets = new List<DataSetModel>{ new DataSetModel
                {Data = new List<int> { verifiedAgents, pendingAgents } } }
            };

            return chart;
        }


        public ChartBase GetCustomersListingsCount()
        {
            var query = _dashboardRepo.GetIqueryableCustomers();

            var CustomersWithListings = query.Count(i => i.Listings.Count > 0);
            var CustomersWithoutListings = query.Count(i => i.Listings.Count == 0);

            ChartBase chart = new PieChart();
            chart.Data = new DataModel
            {
                Labels = new List<string> { "Customers with listings",
                    "Customers without listings" },
                Datasets = new List<DataSetModel>{ new DataSetModel
                {Data = new List<int> { CustomersWithListings, CustomersWithoutListings } } }
            };

            return chart;
        }

        public ChartBase GetCustomersStatus()
        {
            var query = _dashboardRepo.GetIqueryableCustomers();

            var activeCustomers = query.Count(i => i.IsActive);
            var archivedCustomers = query.Count(i => !i.IsActive);

            ChartBase chart = new PieChart();
            chart.Data = new DataModel
            {
                Labels = new List<string> { "Active Customers", "Archived Customers" },
                Datasets = new List<DataSetModel>{ new DataSetModel
                {Data = new List<int> { activeCustomers, archivedCustomers} } }
            };

            return chart;
        }

        public ChartBase GetCustomersViewingsCount()
        {
            var query = _dashboardRepo.GetIqueryableCustomers();

            var CustomersWithViewings = query.Count(i => i.Viewings.Count > 0);
            var CustomersWithoutViewings = query.Count(i => i.Viewings.Count == 0);

            ChartBase chart = new PieChart();
            chart.Data = new DataModel
            {
                Labels = new List<string> { "Customers with Viewings",
                    "Customers without Viewings" },
                Datasets = new List<DataSetModel>{ new DataSetModel
                {Data = new List<int> { CustomersWithViewings, CustomersWithoutViewings } } }
            };

            return chart;
        }


        public ChartBase GetListingsStatus()
        {
            var query = _dashboardRepo.GetIqueryableListings();

            var activeListings = query.Count(i => i.IsActive);
            var archivedListings = query.Count(i => !i.IsActive);

            ChartBase chart = new PieChart();
            chart.Data = new DataModel
            {
                Labels = new List<string> { "Active listings", "Archived listings" },
                Datasets = new List<DataSetModel>{ new DataSetModel
                {Data = new List<int> { activeListings, archivedListings } } }
            };

            return chart;
        }

        public ChartBase GetPastAndCurrentViewingsCount()
        {
            var query = _dashboardRepo.GetIqueryableViewings();

            var activeViewings = query.Count(i => i.StartDate >= DateTime.Now
            && i.IsActive == true);

            var archivedViewings = query.Count(i => i.StartDate < DateTime.Now
            && i.IsActive == true);


            ChartBase chart = new PieChart();
            chart.Data = new DataModel
            {
                Labels = new List<string> { "Future Viewings", "Past Viewings" },
                Datasets = new List<DataSetModel>{ new DataSetModel
                {Data = new List<int> { activeViewings, archivedViewings } } }
            };

            return chart;
        }

        #endregion


        #region charts conversions

        public async Task<List<ClientResponseModel>> GetUsersChartsResponse()
        {
            var usersStatusChart = GetUsersStatus();
            var usersTypeChart = GetUsersTypeCount();
            var agentsVerifiedCharts = GetAgentsVerificationStatus();

            var usersStatusChartRes = _chartClient.GetChartResult(usersStatusChart);
            var usersTypeChartRes = _chartClient.GetChartResult(usersTypeChart);
            var agentsVerifiedChartsRes = _chartClient.GetChartResult(agentsVerifiedCharts);

            await Task.WhenAll(usersStatusChartRes, usersTypeChartRes, agentsVerifiedChartsRes);

            var returnValues = new List<ClientResponseModel>() 
            { usersStatusChartRes.Result, usersTypeChartRes.Result, agentsVerifiedChartsRes.Result };

            return returnValues;
        }

        public async Task<List<ClientResponseModel>> GetCustomersChartsResponse()
        {

            var customerStatus = GetCustomersStatus();
            var customersListings = GetCustomersListingsCount();
            var customersViewings = GetCustomersViewingsCount();

            var customerStatusRes = _chartClient.GetChartResult(customerStatus);
            var customersListingsRes = _chartClient.GetChartResult(customersListings);
            var customersViewingsRes = _chartClient.GetChartResult(customersViewings);

            await Task.WhenAll(customerStatusRes, customersListingsRes, customersViewingsRes);

            var returnValues = new List<ClientResponseModel>()
            { customerStatusRes.Result, customersListingsRes.Result, customersViewingsRes.Result };

            return returnValues;
        }

        public async Task<List<ClientResponseModel>> GetListingsChartsResponse()
        {
            var listingsStatus = GetListingsStatus();
            var listingsStatusRes = _chartClient.GetChartResult(listingsStatus);

            await Task.WhenAll(listingsStatusRes);

            var returnValues = new List<ClientResponseModel>()
            { listingsStatusRes.Result};

            return returnValues;
        }

        public async Task<List<ClientResponseModel>> GetViewingsChartsResponse()
        {
            var viewingsStatus = GetPastAndCurrentViewingsCount();
            var viewingsStatusRes = _chartClient.GetChartResult(viewingsStatus);

            await Task.WhenAll(viewingsStatusRes);

            var returnValues = new List<ClientResponseModel>()
            { viewingsStatusRes.Result};

            return returnValues;
        }
        #endregion


        public void Dispose()
        {
            if (_dashboardRepo != null)
                _dashboardRepo.Dispose();

            if (_chartClient != null)
                _chartClient.Dispose();
        }

    }
}