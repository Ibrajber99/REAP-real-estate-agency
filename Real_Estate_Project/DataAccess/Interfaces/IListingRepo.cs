using Real_Estate_Project.DataAccess.Sql_Classes;
using Real_Estate_Project.Models.Domain_Models.Listing_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Project.DataAccess.Interfaces
{
    public interface IListingRepo : ICrudRepo<Listing>,IDisposable
    {
        Task<List<Heating>> GetAllHeating();

        Task<List<PropertyFeatures>> GetAllFeatures();

        IQueryable<Listing> GetIqueryableListings(
            string searchName = null,decimal minPrice = 0, decimal maxPrice = 0
            , int numofBeds = 0, int numOfBaths = 0,int numOfStories = 0);

        Task<ListingImage> GetListingImage(int imageId);

        Task<ListingImage> GetListingThumbnail(int listingId);

    }
}
