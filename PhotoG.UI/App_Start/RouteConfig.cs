using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoG.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute("UnhandledExceptions", "Error", new { controller = "Error", action = "Index" });

            routes.MapRoute(
                name: "UnlinkPhoto",
                url: "photo/unlinkphoto/{albumId}/{photoId}",
                defaults: new { controller = "photo", action = "UnlinkPhoto", albumId = UrlParameter.Optional, photoId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GetAlbumByTitle",
                url: "album/title/{title}",
                defaults: new { controller = "photo", action = "AlbumPhotosByTitle", title = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "PhotoDetails",
                url: "photo/details/{id}",
                defaults: new { controller = "photo", action = "PhotoDetails", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "LinkPhotoTo",
                url: "photo/linkto",
                defaults: new {controller = "photo", action = "LinkPhotoToAlbum"}
            );
            
            routes.MapRoute(
                name: "GetUserPhotos",
                url: "photo/userphotos",
                defaults: new { controller = "photo", action = "GetUserPhotos" }
            );

            routes.MapRoute(
                name: "GetUserAlbums",
                url: "album/useralbums",
                defaults: new { controller = "album", action = "GetUserAlbums" }
            );

            routes.MapRoute(
                name: "InsertOrUpdate",
                url: "album/insertorupdate",
                defaults: new { controller = "album", action = "InsertOrUpdate" }
            );
            
            routes.MapRoute(
                name: "AlbumPhotos",
                url: "album/photos/{albumId}",
                defaults: new { controller = "photo", action = "AlbumPhotos", albumId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
