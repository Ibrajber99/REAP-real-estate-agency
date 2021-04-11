using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Real_Estate_Project.View_Uitlity_Model_Helpers
{
    public abstract class ImageHelper
    {
        private const int MAX_FILE_SIZE = 3145728;

        private bool isValidFileSize(HttpPostedFileBase file)
        {
            return file.ContentLength <= MAX_FILE_SIZE && file.ContentLength > 0;
        }

        private bool isFileTypeImage(HttpPostedFileBase file)
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



    }
}