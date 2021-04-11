using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models;
using Real_Estate_Project.Models.Domain_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Real_Estate_Project.DataAccess.Sql_Classes
{
    public class SqlViewing : IViewingRepo
    {
        private ApplicationDbContext _context;

        public SqlViewing()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<int> Create(Viewing viewing)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Viewings.Attach(viewing);
                context.Viewings.Add(viewing);

                return await context.SaveChangesAsync();
            }
        }

        public async Task<Viewing> Get(int id)
        {
            var viewing = _context.Viewings
                .Include(c => c.Customer)
                 .Include(u => u.ViewingHost)
                 .Include(l => l.Listing)
                 .Include(u => u.UserCreator)
                 .Include(u => u.UserUpdator);



            return await viewing.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ID == id);

        }

        public async Task<List<Viewing>> GetAll()
        {
            using (var context = new ApplicationDbContext())
            {
                var viewings = context.Viewings
                .Include(c => c.Customer)
                .Include(u => u.ViewingHost)
                .Include(l => l.Listing)
                .Include(l => l.Listing.ListingAddress)
                .Where(v => v.IsActive == true);

                return await viewings.AsNoTracking().ToListAsync();
            }
        }


        public async Task<int> Update(Viewing viewing)
        {
            var viewingToModify = _context.Viewings.Find(viewing.ID);
            viewingToModify.StartDate = viewing.StartDate;
            viewingToModify.EndDate = viewing.EndDate;
            viewingToModify.UpdatedDate = viewing.UpdatedDate;
            viewingToModify.UserUpdatorId = viewing.UserUpdatorId;
            viewingToModify.IsActive = viewing.IsActive;

            viewingToModify.ViewingHost =
            await _context.siteUsers.FirstOrDefaultAsync
            (u => u.ID == viewing.ViewingHost.ID);

            viewingToModify.Customer =
            await _context.Customers.FirstOrDefaultAsync
            (u => u.ID == viewing.Customer.ID);

            viewingToModify.Listing = 
                await _context.Listing.FirstOrDefaultAsync
                (u => u.ID == viewing.Listing.ID);


            var entry = _context.Entry(viewingToModify);
            entry.State = EntityState.Modified;
            
            return await _context.SaveChangesAsync();
        }


        public async Task<int> Delete(Viewing viewing)
        {
            return await Update(viewing);
        }

        public async Task<int> Delete(int id)
        {
            //DO not use it
            var viewing = _context.Viewings.Find(id);
            viewing.IsActive = false;
            return await Delete(viewing);
        }

        public async Task<List<Customer>> GetCustomers()
        {
            using (var context = new ApplicationDbContext())
            {
                var customers = context.Customers
                    .Where(cus => cus.IsActive == true).AsNoTracking();

                return await customers.ToListAsync();
            }
        }

        public async Task<List<OperatingUser>> GetAgents()
        {
            using (var context = new ApplicationDbContext())
            {
                var usersList = context.siteUsers.
                Where(u => u.RoleID.Equals(RoleNames.AGENT)
                && u.IsActive == true && u.IsVerified == true);

                return await usersList.ToListAsync();
            }
        }

        public async Task<List<Listing>> GetListings()
        {
            using (var context = new ApplicationDbContext())
            {
                var lisitngList = context.Listing
                .Where(l => l.IsActive == true);

                return await lisitngList.ToListAsync();
            }
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var customer = context.Customers;

                return await customer
                       .AsNoTracking()
                       .FirstOrDefaultAsync
                       (cus => cus.ID == id); ;
            }   
        }

        public async Task<OperatingUser> GetAgentById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var findUser = context.siteUsers;

                return await findUser
                    .AsNoTracking().FirstOrDefaultAsync(user => user.ID == id);
            }
                
        }

        public async Task<Listing> GetListingById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var findListing = context.Listing;

                return await findListing
                    .Include(l => l.ListingAddress)
                    .AsNoTracking().FirstOrDefaultAsync(lis => lis.ID == id);
            }
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();

            }
        }

        public async Task<List<Viewing>> GetViewingsByDate
        (DateTime fromDate, DateTime tillDate)
        {
            var fromToday = fromDate.Date;
            var tillDay = tillDate.Date;

            var query = _context.Viewings.Where(v =>
               DbFunctions.TruncateTime(v.StartDate) >= fromToday
               && DbFunctions.TruncateTime(v.StartDate) <= tillDay
               && v.IsActive == true)
                .Include(c => c.Customer)
                .Include(u => u.ViewingHost)
                .Include(l => l.Listing);


            return await query.AsNoTracking().ToListAsync();
        }
    }
}