using Real_Estate_Project.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.DataAccess.Factories
{
    public static class ListingRepoFactory
    {
        public static Func<IListingRepo> CreateListingRepo;

        public static IListingRepo GetListingRepo()
        {
            return CreateListingRepo();
        }
    }
}