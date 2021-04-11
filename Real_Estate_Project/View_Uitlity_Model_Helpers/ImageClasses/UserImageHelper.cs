using Real_Estate_Project.DataAccess.Data_Models;
using Real_Estate_Project.Models.Domain_Models.OperatingUser_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Real_Estate_Project.View_Uitlity_Model_Helpers
{
    public class UserImageHelper : IUserImageHelper
    {
        private const int MAX_FILE_SIZE = 3145728;

        public  List<OperatingUserImage> GetLicnesesImageFile
            (List<HttpPostedFileBase> fileuploads)
        {
            var userImagesRes = new List<OperatingUserImage>();

            if (fileuploads.Count > 0)
            {
                foreach (var file in fileuploads)
                {
                    var imageFile = GetUserImageModel(file);

                    userImagesRes.Add(imageFile);
                }
            }

            return userImagesRes;
        }


        public  OperatingUserImage GetUserImageModel(HttpPostedFileBase upload)
        {
            var UserImage = new OperatingUserImage
            {
                FileName = Path.GetFileName(upload.FileName),
                ContentType = upload.ContentType,
                Content = GetFileBytes(upload)
            };
            return UserImage;
        }

        
        public  void SetUserProfileImage(HttpPostedFileBase imageFile,
            OperatingUser user)
        {
            var userImageModel = GetUserImageModel(imageFile);

            if (user.ProfileImage == null)
            {
                userImageModel.UserCreatorId = user.ID;
                userImageModel.DateAdded = DateTime.Now;
                userImageModel.FileType = UserFileType.Photo;
                userImageModel.LicenseExpiryDate = DateTime.Now;

                user.ProfileImage = userImageModel;

            }
            else
            {
                user.ProfileImage.FileName = userImageModel.FileName;
                user.ProfileImage.Content = userImageModel.Content;
                user.ProfileImage.ContentType = userImageModel.ContentType;
            }
        }

        
        public  bool IsDrivingLicenseValid(HttpPostedFileBase imageFile,
             DateTime? expireyDate, ModelStateDictionary modelState)
        {
            if (!isLicenseDateValid(expireyDate))
            {
                modelState.AddModelError(string.Empty,
                    "Please Licesne expiry date is not valid");

                return false;
            }

            if (!isValidFileUpload(imageFile))
            {
                modelState.AddModelError(string.Empty,
                     "Please Provide a valid File type");

                return false;
            }

            return true;
        }

        
        public  void SetProfileDriverLicense(HttpPostedFileBase imageFile,
            OperatingUser user, DateTime? expireyDate)
        {

            var userLicense =
                 GetUserImageModel(imageFile);

            userLicense.UserCreatorId = user.ID;
            userLicense.DateAdded = DateTime.Now;
            userLicense.FileType = UserFileType.DRIVER_LICENSE;
            userLicense.LicenseExpiryDate = expireyDate.Value;

            user.DriverLicenses.Add(userLicense);
        }


        public bool isValidFileSize(HttpPostedFileBase file)
        {
            return file.ContentLength <= MAX_FILE_SIZE && file.ContentLength > 0;
        }

        public bool isFileTypeImage(HttpPostedFileBase file)
        {
            try
            {
                Image img = Image.FromStream(file.InputStream);

                if (ImageFormat.Jpeg.Equals(img.RawFormat))
                    return true;

                if (ImageFormat.Png.Equals(img.RawFormat))
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return false;
        }

        public bool isValidFileUpload(HttpPostedFileBase file)
        {
            return isValidFileSize(file) && isFileTypeImage(file);
        }

        public bool isValidFileUpload(List<HttpPostedFileBase> fileuploads)
        {
            if (fileuploads.Count > 0)
            {
                return fileuploads.TrueForAll(d => isValidFileUpload(d));
            }

            return false;
        }

        public byte[] GetFileBytes(HttpPostedFileBase upload)
        {
            byte[] content;
            var stream = upload.InputStream;

            using (var reader = new BinaryReader(stream))
            {
                stream.Position = 0;

                content = reader.ReadBytes(upload.ContentLength);
            }

            return content;
        }

        public bool isLicenseDateValid(DateTime? expiryDate)
        {
            if (!expiryDate.HasValue)
                return false;

            var date = expiryDate.Value;

            return date.Date.Year > DateTime.Now.Year;
        }
    }
}