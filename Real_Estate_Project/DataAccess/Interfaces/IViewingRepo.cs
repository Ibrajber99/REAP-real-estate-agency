using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.DataAccess.Sql_Classes;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Project.DataAccess.Interfaces
{
    public interface IViewingRepo : ICrudRepo<Viewing>, IDisposable
    {
        Task<List<Customer>> GetCustomers();

        Task<List<OperatingUser>> GetAgents();

        Task<List<Listing>> GetListings();

        Task<Customer> GetCustomerById(int id);

        Task<OperatingUser> GetAgentById(int id);

        Task<Listing> GetListingById(int id);

        Task<List<Viewing>> GetViewingsByDate(DateTime fromDate, DateTime tillDate);

    }
}
