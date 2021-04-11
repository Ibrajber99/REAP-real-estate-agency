using PagedList;
using Real_Estate_Project.DataAccess.Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.ViewModels.OperatingUser_ViewModels
{
    public class VerificationListViewModel
    {
        public OperatingUser InputModel { get; set; }

        public OperatingUser CurrentViewingModel { get; set; }


        public IPagedList<OperatingUser> usersList;

        public string IsVerifiedSearchParam { get; set; }

        public string CurrentSort { get; set; }
    }
}