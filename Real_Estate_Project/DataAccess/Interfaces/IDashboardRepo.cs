using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Linq;


namespace Real_Estate_Project.DataAccess.Interfaces
{
    public interface IDashboardRepo : IDisposable
    {
        //Iqueryables
        IQueryable<OperatingUser> GetIqueryableUsers();

        IQueryable<Customer> GetIqueryableCustomers();

        IQueryable<Listing> GetIqueryableListings();

        IQueryable<Viewing> GetIqueryableViewings();

    }
}
