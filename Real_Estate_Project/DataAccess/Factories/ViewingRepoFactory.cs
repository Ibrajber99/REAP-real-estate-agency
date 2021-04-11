using Real_Estate_Project.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.DataAccess.Factories
{
    public static class ViewingRepoFactory
    {
        public static Func<IViewingRepo> CreateViewingRepo;

        public static IViewingRepo GetViewingRepo()
        {
            return CreateViewingRepo();
        }
    }
}