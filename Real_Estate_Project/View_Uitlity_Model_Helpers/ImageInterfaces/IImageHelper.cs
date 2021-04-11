using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Real_Estate_Project.View_Uitlity_Model_Helpers
{
    public interface IImageHelper
    {
        bool isValidFileSize(HttpPostedFileBase file);

        bool isFileTypeImage(HttpPostedFileBase file);

        bool isValidFileUpload(HttpPostedFileBase file);

        bool isValidFileUpload(List<HttpPostedFileBase> fileuploads);

        byte[] GetFileBytes(HttpPostedFileBase upload);

    }
}
