using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using Real_Estate_Project.Authorization_Utilities;
using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.ViewModels.OperatingUser_ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.ViewModels
{
    public class OperatingUserViewModel
    {
        public OperatingUserViewModel()
        {
            InputModel = new OperatingUser();
            rolesList = new List<IdentityRole>();
            SearchModel = new UserSearchViewModel();
        }


        public OperatingUser InputModel { get; set; }

        public IPagedList<OperatingUser> usersList;

        public List<IdentityRole> rolesList;

        public UserSearchViewModel SearchModel { get; set; }
    }
}