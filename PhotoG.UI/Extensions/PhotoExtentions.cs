
using System.IO;
using System.Web;
using PhotoG.UI.Models;

namespace PhotoG.UI.Extensions
{
    public static class PhotoExtentions
    {
        public static bool TryGetImageFromRequest(this HttpContextBase httpContext, PhotoModel model)
        {
            HttpPostedFileBase file = null;
            
            if (httpContext.Request.Files != null && httpContext.Request.Files.Count > 0)
                file = httpContext.Request.Files[0];

            if (file == null) return false;
            
            model.MapImage(file);
            
            return true;
        }

        public static void MapImage(this PhotoModel model, HttpPostedFileBase file)
        {
            byte[] imageData;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                imageData = binaryReader.ReadBytes(file.ContentLength);
            }

            model.Image = imageData;
            model.ImageType = file.ContentType;
            model.ImageSize = imageData.Length;
        }

        public static string ValidateImage(this PhotoModel model)
        {
            if (model.ImageType != null &&
                model.ImageType.ToLower() != "image/jpeg" &&
                model.ImageType.ToLower() != "image/png" &&
                model.ImageType.ToLower() != "image/jpg")
            {
                return "Photo has incorrect type, correct type is jpeg, png, jpg";
            }

            if (model.ImageSize != 0 && model.ImageSize >= (1024*512))
                return "Maximum photo size is 500KB";

            return string.Empty;
        }
    }
}