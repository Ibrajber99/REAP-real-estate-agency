using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.DataAccess.Interfaces;
using Real_Estate_Project.Models;
using Real_Estate_Project.Models.Domain_Models.OperatingUser_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Real_Estate_Project.DataAccess
{
    public class SqlOperatingUsers : IOperatingUserRepo
    {
        private ApplicationDbContext _context;

        public SqlOperatingUsers()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<int> Create(OperatingUser item)
        {
            throw new NotImplementedException();
        }

        public async Task<OperatingUser> Get(int id)
        {
            var findUser = _context.siteUsers
                .Include(u => u.UserUpdator)
                .Include(u => u.UserCreator)
                .Include(i => i.ProfileImage)
                .Include(d => d.DriverLicenses)
                .Where(user => user.ID == id);

            return await findUser.FirstOrDefaultAsync();
        }

        public async Task<List<OperatingUser>> GetAll()
        {
            var userList = _context.siteUsers.Where
                      (u => u.IsActive == true)
                      .Include(i => i.ProfileImage);

            return await userList.AsNoTracking().ToListAsync();
        }

        public async Task<List<OperatingUser>> GetAllByRole(string roleName)
        {
            var usersByRoles = _context.siteUsers.
            Where(user => user.RoleID.Equals(roleName)
            && user.IsActive == true);

            return await usersByRoles.AsNoTracking().ToListAsync();

        }

        public async Task<List<OperatingUser>> GetAllWithDriverLicense()
        {
            var userList = _context.siteUsers.Where
                     (u => u.IsActive == true)
                     .Include(i => i.ProfileImage)
                     .Include(i => i.DriverLicenses)
                     .Where(m => m.DriverLicenses.Count > 0);

            return await userList.AsNoTracking().ToListAsync();
        }

        public IQueryable<OperatingUser> GetIqueryAbleUsers
            (string searchname=null,bool includeOnlyAgents = false)
        {
            var userList = _context.siteUsers
                .Where(i => i.IsActive).Include(m => m.ProfileImage);

            if (!string.IsNullOrEmpty(searchname))
            {
                userList = userList.
                    Where(l => l.FirstName.Contains(searchname)
                           || l.LastName.Contains(searchname));
            }

            if (includeOnlyAgents)
            {
                userList = userList.Where(m => m.DriverLicenses.Count > 0
                && m.RoleID == RoleNames.AGENT);
            }

            return userList;
        }


        public async Task<int> Update(OperatingUser user)
        {
          
            var entry = _context.Entry(user);
            entry.State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveUserImages(OperatingUser user)
        {
            //if there is new image...
            if (user.DriverLicenses.Any(i => i.ID == 0))
            {
                foreach (var license in user.DriverLicenses)
                {
                    if (license.ID == 0)
                    {
                        user.IsVerified = false;
                        _context.Entry(license).State = EntityState.Added;
                    }
                    else
                    {
                        license.IsArchived = true;
                        license.DateArchived = DateTime.Now;
                        _context.Entry(license).State = EntityState.Modified;
                    }
                }
            }

            if (user.ProfileImage != null)//If they didn't add a profile image (Edge case)
            {
                if (user.ProfileImage.ID > 0)
                {
                    var findImage = _context.UsersProfileImages.Find(user.ProfileImage.ID);
                    if (findImage != null)
                    {
                        findImage = user.ProfileImage;
                        user.ProfileImage.UserUpdatorId = user.ID;

                        _context.Entry(findImage).State = EntityState.Modified;
                    }
                }
                else
                {
                    _context.Entry(user.ProfileImage).State = EntityState.Added;
                }
            }

            var entry = _context.Entry(user);
            entry.State = EntityState.Modified;


            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(OperatingUser user)
        {
            _context.Entry(user).State =
              EntityState.Modified;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var userFound = _context.siteUsers.
            FirstOrDefault(x => x.ID == id);
            

            return await Delete(userFound);
        }

        public List<Viewing> GetAgentRelatedViewings(int id)
        {
            var viewings = _context.Viewings.Include(u => u.ViewingHost)
                .Include(c => c.Customer).Include(l => l.Listing)
                .Where(u => u.ViewingHost.ID == id).ToList();

            return viewings;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public async Task<OperatingUserImage> GetUserProfileImage(int id)
        {
            var userImage = _context.UsersProfileImages
                .FirstOrDefaultAsync(x => x.ID == id);

            return await userImage;
        }

        public async Task<OperatingUserImage> GetUserLatestLicesne(int id)
        {
            var userLicense = _context.siteUsers.Where(i => i.ID == id);

            var filterRes = userLicense.Where(i => i.ID == id)
                .Select(m => m.DriverLicenses
                .Where(i => i.FileType == UserFileType.DRIVER_LICENSE).FirstOrDefault(l => !l.IsArchived))
                .FirstOrDefaultAsync(l => !l.IsArchived);

            return await filterRes;
        }
    }
}