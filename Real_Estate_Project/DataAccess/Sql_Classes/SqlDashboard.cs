using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models;
using Real_Estate_Project.Models.Dashboard_Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Real_Estate_Project.DataAccess.Sql_Classes
{
    public class SqlDashboard : IDashboardRepo
    {

        private ApplicationDbContext _dashboardContext;

        public SqlDashboard()
        {
            _dashboardContext = new ApplicationDbContext();
        }


        public void Dispose()
        {
            if(_dashboardContext != null)
            {
                _dashboardContext.Dispose();
            }
        }


        public IQueryable<OperatingUser> GetIqueryableUsers()
        {
            var q = _dashboardContext.siteUsers.AsQueryable();
            return q;
        }

        public IQueryable<Customer> GetIqueryableCustomers()
        {
            var q = _dashboardContext.Customers.AsQueryable();
            return q;
        }

        public IQueryable<Listing> GetIqueryableListings()
        {
            var q = _dashboardContext.Listing.AsQueryable();
            return q;
        }

        public IQueryable<Viewing> GetIqueryableViewings()
        {
            var q = _dashboardContext.Viewings.AsQueryable();
            return q;
        }
    }
}