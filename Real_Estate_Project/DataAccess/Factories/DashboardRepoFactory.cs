using Real_Estate_Project.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.DataAccess.Factories
{
    public static class DashboardRepoFactory
    {
        public static Func<IDashboardRepo> CreateDashboardRepo;

        public static IDashboardRepo GetDashboardRepo()
        {
            return CreateDashboardRepo();
        }
    }
}