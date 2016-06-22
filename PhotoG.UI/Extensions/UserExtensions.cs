using System.Configuration;
using System.Linq;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using PhotoG.BL.Services;
using PhotoG.Infrastructure.Identity;

namespace PhotoG.UI.Extensions
{
    public static class UserExtensions
    {
        public static bool CanUserAddAlbums(this IPrincipal user, IAlbumService albumService)
        {
            if (user.Identity.IsAuthenticated && user.IsInRole(Roles.Premium.ToString()))
                return true;

            int maxAlbumCount;
            if (!int.TryParse(ConfigurationManager.AppSettings["MaxAlbumCount"], out maxAlbumCount))
                return false;

            var albums = albumService.GetAlbumsByUserId(user.Identity.GetUserId());

            return albums.Count() < maxAlbumCount;
        }

        public static bool CanUserAddPhotos(this IPrincipal user, IPhotoService photoService)
        {
            if (user.Identity.IsAuthenticated && user.IsInRole(Roles.Premium.ToString()))
                return true;

            int maxPhotoCount;
            if (!int.TryParse(ConfigurationManager.AppSettings["MaxPhotoCount"], out maxPhotoCount))
                return false;

            var photos = photoService.GetPhotosByUserId(user.Identity.GetUserId());

            return photos.Count() < maxPhotoCount;
        }
    }
}