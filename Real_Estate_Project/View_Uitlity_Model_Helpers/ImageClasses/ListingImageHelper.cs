using Real_Estate_Project.Models.Domain_Models.Listing_Models;
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
    public class ListingImageHelper : IListingImageHelper
    {
        private const int MAX_FILE_SIZE = 3145728;

        private const int THUMBNAIL_TARGET_SIZE = 20000;//20 kbs

        private const int MAX_IMAGES_LIMIT = 7;


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

        
        public List<ListingImage> GetListingImageFile(List<HttpPostedFileBase> fileuploads)
        {
            var listingImagesRes = new List<ListingImage>();

            if (fileuploads.Count > 0)
            {
                foreach (var file in fileuploads)
                {
                    var imageFile = GetListingImageFile(file);

                    listingImagesRes.Add(imageFile);
                }
            }

            return listingImagesRes;
        }

        
        public ListingImage GetListingImageFile(HttpPostedFileBase upload)
        {
            var listingImage = new ListingImage
            {
                FileName = Path.GetFileName(upload.FileName),
                ContentType = upload.ContentType,
                Content = GetFileBytes(upload)
            };
            var contentArray = listingImage.Content;

            listingImage.ThumbnailContent =
                GetThumbnailFileBytes(contentArray);

            return listingImage;
        }

        
        public bool IsPostedFilesListEmpty(List<HttpPostedFileBase> imageList)
        {
            return imageList.Count <= 0 || imageList.FirstOrDefault() == null;
        }

        
        public bool HasImageListExceededSizeLimit(List<HttpPostedFileBase> imageList)
        {
            return imageList.Count > MAX_IMAGES_LIMIT;
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


        public byte[] GetThumbnailFileBytes(byte[] originalBytes)
        {
            var sizeToCropBytes = originalBytes;
            double scale = 1f;

            if (originalBytes.Length > THUMBNAIL_TARGET_SIZE)//->target size is 20KB
            {
                MemoryStream inputMemoryStream = new MemoryStream(sizeToCropBytes);
                Image fullsizeImage = Image.FromStream(inputMemoryStream);


                while (sizeToCropBytes.Length > THUMBNAIL_TARGET_SIZE)
                {
                    Bitmap fullSizeBitmap = new Bitmap(fullsizeImage,
                        new Size((int)(fullsizeImage.Width * scale),
                        (int)(fullsizeImage.Height * scale)));

                    MemoryStream resultStream = new MemoryStream();

                    fullSizeBitmap.Save(resultStream, fullsizeImage.RawFormat);

                    sizeToCropBytes = resultStream.ToArray();
                    resultStream.Dispose();
                    resultStream.Close();

                    scale -= 0.05f;

                }

                return sizeToCropBytes;

            }

            return originalBytes;

        }


        public void ArchiveCheckedImages(List<SelectListItem> selectList,
            List<ListingImage> imagesList)
        {
            foreach (var image in selectList)
            {
                if (image.Selected)
                {
                    var selectedImage = imagesList.FirstOrDefault
                        (m => m.ID.ToString() == image.Value);

                    if (selectedImage != null)
                    {
                        selectedImage.IsArchived = true;
                    }
                }
            }
        }


        public void SetModelImages
            (Listing listingModel, List<HttpPostedFileBase> imageList)
        {
            var imageFiles = GetListingImageFile(imageList);

            foreach (var image in imageFiles)
            {
                listingModel.ImagesContent.Add(image);
            }
        }

        public void SetListingDefaultImage(int imageId, Listing listing)
        {
            if (listing.ImagesContent != null)
            {
                listing.ImagesContent.ToList()
                    .ForEach(i => i.IsDefaultImage = false);

                var imageFound = listing.ImagesContent
                    .FirstOrDefault(i => i.ID == imageId);

                if (imageFound != null)
                    imageFound.IsDefaultImage = true;
            }
        }

    }
}