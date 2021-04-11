using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.DataAccess.Sql_Classes;
using Real_Estate_Project.Models.Domain_Models.OperatingUser_Models;
using Real_Estate_Project.Models.Domain_Models.Viewing_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Project.DataAccess.Interfaces
{
    public interface IOperatingUserRepo : ICrudRepo<OperatingUser>,IDisposable
    {
        Task<List<OperatingUser>> GetAllByRole(string roleName);

        Task<List<OperatingUser>> GetAllWithDriverLicense();

        List<Viewing> GetAgentRelatedViewings(int id);

        Task<OperatingUserImage> GetUserProfileImage(int id);

        Task<OperatingUserImage> GetUserLatestLicesne(int id);

        Task<int> SaveUserImages(OperatingUser user);

        IQueryable<OperatingUser> GetIqueryAbleUsers
            (string searchname = null, bool includeOnlyAgents = false);
    }
}
