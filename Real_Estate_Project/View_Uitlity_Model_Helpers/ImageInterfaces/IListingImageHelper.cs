using Real_Estate_Project.Models.Domain_Models.Listing_Models;
using Real_Estate_Project.Models.Listing_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Real_Estate_Project.View_Uitlity_Model_Helpers
{
    public interface IListingImageHelper : IImageHelper
    {
        List<ListingImage> GetListingImageFile(List<HttpPostedFileBase> fileuploads);

        ListingImage GetListingImageFile(HttpPostedFileBase upload);

        bool IsPostedFilesListEmpty(List<HttpPostedFileBase> imageList);

        bool HasImageListExceededSizeLimit(List<HttpPostedFileBase> imageList);

        byte[] GetThumbnailFileBytes(byte[] originalBytes);

        void ArchiveCheckedImages(List<SelectListItem> selectList,
            List<ListingImage> imagesList);


        void SetModelImages
            (Listing listingModel, List<HttpPostedFileBase> imageList);


        void SetListingDefaultImage(int imageId, Listing listing);
    }
}
