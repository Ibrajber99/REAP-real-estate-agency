using Real_Estate_Project.Models.Dashboard_Models.ChartClient;
using Real_Estate_Project.ViewModels.OperatingUser_ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.ViewModels.Dashboard_View_Models
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            UsersCharts = new List<ClientResponseModel>();
            CustomersCharts = new List<ClientResponseModel>();
            ListingsCharts = new List<ClientResponseModel>();
            ViewingsCharts = new List<ClientResponseModel>();
        }

        public List<ClientResponseModel> UsersCharts { get; set; }

        public List<ClientResponseModel> CustomersCharts { get; set; }

        public List<ClientResponseModel> ListingsCharts { get; set; }

        public List<ClientResponseModel> ViewingsCharts { get; set; }

    }
}