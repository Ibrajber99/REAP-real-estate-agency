using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models.OperatingUser_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Real_Estate_Project.View_Uitlity_Model_Helpers
{
    public interface IUserImageHelper : IImageHelper
    {
       List<OperatingUserImage> GetLicnesesImageFile
            (List<HttpPostedFileBase> fileuploads);


        OperatingUserImage GetUserImageModel(HttpPostedFileBase upload);


        bool isLicenseDateValid(DateTime? expiryDate);

        void SetUserProfileImage(HttpPostedFileBase imageFile,
            OperatingUser user);

        bool IsDrivingLicenseValid(HttpPostedFileBase imageFile,
             DateTime? expireyDate, ModelStateDictionary modelState);

        void SetProfileDriverLicense(HttpPostedFileBase imageFile,
            OperatingUser user, DateTime? expireyDate);
    }
}
