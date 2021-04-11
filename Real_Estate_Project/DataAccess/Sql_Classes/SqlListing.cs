using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models;
using Real_Estate_Project.Models.Domain_Models.Listing_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace Real_Estate_Project.DataAccess.Sql_Classes
{
    public class SqlListing : IListingRepo
    {
        private ApplicationDbContext _listingContext;

        public SqlListing()
        {
            _listingContext = new ApplicationDbContext();
        }

        public async Task<int> Create(Listing listing)
        {
            using (var context = new ApplicationDbContext())
            {
                if (listing.ImagesContent.Count > 0)
                {
                    foreach (var image in listing.ImagesContent)
                    {
                        image.DateAdded = DateTime.Now;
                        image.UserCreatorId = listing.UserCreatorId;
                    }
                }


                listing.Heating.ForEach(h => context.HeatingType.Attach(h));
                listing.Features.ForEach(f => context.ListingFeatures.Attach(f));


                context.Listing.Add(listing);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<List<Listing>> GetAll()
        {

            var listingList = _listingContext.Listing
              .Include(add => add.ListingAddress)
              .Include(img => img.ImagesContent)
              .Where(l => l.IsActive == true)
              .OrderByDescending(m => m.ID);


            return await listingList.AsNoTracking().ToListAsync();
        }

        public async Task<Listing> Get(int id)
        {
            var findListing = await _listingContext.Listing
                        .Include(h => h.Heating)
                        .Include(c => c.Features)
                        .Include(u => u.UserUpdator)
                        .Include(u => u.UserCreator)
                        .Where(lis => lis.ID == id)
                        .AsNoTracking().FirstOrDefaultAsync();

            var activeImages = _listingContext.ListingImages
             .Where(c => c.IsArchived == false && c.Listing.ID == id)
             .AsNoTracking().ToList();

            var result = findListing;
            result.ImagesContent = activeImages;

            return result;
        }

        public async Task<int> Update(Listing listing)
        {

            var heatList = listing.Heating;
            var featureList = listing.Features;
            var imagesContent = listing.ImagesContent;


            var listingToUpdate = _listingContext.Listing.
            Include(x => x.Heating).Include(x => x.Features).
            FirstOrDefault(s => s.ID == listing.ID);

            #region Copying Props
            listingToUpdate.ListingAddress = listing.ListingAddress;
            listingToUpdate.ListingAddressID = listing.ListingAddressID;
            listingToUpdate.Customer = listing.Customer;
            listingToUpdate.CustomerID = listing.CustomerID;
            listingToUpdate.AssociatedAgent = listing.AssociatedAgent;
            listingToUpdate.AssociatedAgentID = listing.AssociatedAgentID;
            listingToUpdate.NumOfBaths = listing.NumOfBaths;
            listingToUpdate.NumOfBeds = listing.NumOfBeds;
            listingToUpdate.NumOfStories = listing.NumOfStories;
            listingToUpdate.Price = listing.Price;
            listingToUpdate.SquareFootage = listing.SquareFootage;
            listingToUpdate.SummaryFeature = listing.SummaryFeature;
            listingToUpdate.UserCreator = listing.UserCreator;
            listingToUpdate.UserCreatorId = listing.UserCreatorId;
            listingToUpdate.UserUpdator = listing.UserUpdator;
            listingToUpdate.UserUpdatorId = listing.UserUpdatorId;
            listingToUpdate.DateCreated = listing.DateCreated;
            listingToUpdate.DateUpdated = listing.DateUpdated;
            listingToUpdate.CityArea = listing.CityArea;
            #endregion

            listingToUpdate.Heating = new List<Heating>();
            listingToUpdate.Features = new List<PropertyFeatures>();
            listingToUpdate.ImagesContent = new List<ListingImage>();

            foreach (var fileImage in imagesContent)
            {
                if (fileImage.ID <= 0)
                {
                    fileImage.DateAdded = DateTime.Now;
                    fileImage.UserCreatorId = listing.UserCreatorId;

                    _listingContext.Entry(fileImage).State = EntityState.Added;
                    listingToUpdate.ImagesContent.Add(fileImage);
                }
                else
                {
                    var findImage = _listingContext.ListingImages.Find(fileImage.ID);

                    _listingContext.ListingImages.Attach(findImage);

                    findImage.IsDefaultImage = fileImage.IsDefaultImage;

                    if (fileImage.IsArchived)
                    {
                        findImage.IsArchived = true;
                        findImage.IsDefaultImage = false;
                        findImage.DateArchived = DateTime.Now;
                        findImage.UserUpdatorId = listing.UserUpdatorId;
                    }

                    _listingContext.Entry(findImage).State = EntityState.Modified;
                    listingToUpdate.ImagesContent.Add(findImage);
                }
            }

            heatList.ForEach(h => listingToUpdate.Heating.Add
            (_listingContext.HeatingType.Find(h.ID)));

            featureList.ForEach(f => listingToUpdate.Features.Add
            (_listingContext.ListingFeatures.Find(f.ID)));

            var entry = _listingContext.Entry(listingToUpdate);
            entry.State = EntityState.Modified;

            return await _listingContext.SaveChangesAsync();

        }

        public async Task<List<Heating>> GetAllHeating()
        {

            using (var context = new ApplicationDbContext())
            {
                var heatingList = context.HeatingType;

                return await heatingList.AsNoTracking().ToListAsync();
            }

        }

        public async Task<List<PropertyFeatures>> GetAllFeatures()
        {
            using (var context = new ApplicationDbContext())
            {
                var featuresList = context.ListingFeatures;

                return await featuresList.AsNoTracking().ToListAsync();
            }
        }

        public async Task<int> Delete(Listing listing)
        {
            _listingContext.Entry(listing).State
                = EntityState.Modified;

            return await _listingContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            //Do not use
            var listing = _listingContext.Listing.Find(id);
            listing.IsActive = false;

            return await Delete(listing);
        }

        public void Dispose()
        {
            if (_listingContext != null)
            {
                _listingContext.Dispose();
            }
        }

        public IQueryable<Listing> GetIqueryableListings
            (string searchName = null,decimal minPrice =0,decimal maxPrice =0
            ,int numofBeds =0, int numOfBaths =0,int numOfStories =0)
        {
            var query = _listingContext.Listing
                .Where(l => (string.IsNullOrEmpty(searchName))
                                    ||
                l.ListingAddress.Municipality.Contains(searchName));

            if(minPrice > 0 && maxPrice > 0)
            {
                query = query.Where
                (i => i.Price >= minPrice && i.Price <= maxPrice);
            }

            if(numofBeds > 0)
            {
                query = query.Where
                  (i => i.NumOfBeds == numofBeds);
            }

            if(numOfBaths > 0)
            {
                query = query.Where
                    (i => i.NumOfBaths == numOfBaths);
            }

            if(numOfStories > 0)
            {
                query = query.Where
                    (i => i.NumOfStories == numOfStories);
            }


            return query
                .Include(c => c.ListingAddress)
                .Where(i => i.IsActive == true)
                .OrderBy(l => l.ID);
        }


        public async Task<ListingImage> GetListingImage(int imageId)
        {
            var image = _listingContext.ListingImages.
                FirstOrDefaultAsync(i => i.ID == imageId);

            return await image;
        }

        public async Task<ListingImage> GetListingThumbnail(int listingId)
        {
            var image = _listingContext
                          .ListingImages.Where(i => i.Listing.ID == listingId);

            var res = await image.Where(i => i.IsDefaultImage &&
            !i.IsArchived && i.Listing.ID == listingId)
                .FirstOrDefaultAsync();

            if (res == null)
            {
                res = await _listingContext.ListingImages
                    .Where(i => i.Listing.ID == listingId && !i.IsArchived)
                    .FirstOrDefaultAsync();
            }

            return res;
        }
    }
}