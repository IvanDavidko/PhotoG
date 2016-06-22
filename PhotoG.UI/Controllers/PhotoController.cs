using System;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoG.BL.Services;
using PhotoG.DAL.Entities;
using PhotoG.Infrastructure.Logging;
using PhotoG.UI.Extensions;
using PhotoG.UI.Models;

namespace PhotoG.UI.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IAlbumService _albumService;
        private readonly ILogger _logger;

        public PhotoController(
            IPhotoService photoService,
            IAlbumService albumService,
            ILogger logger)
        {
            _photoService = photoService;
            _albumService = albumService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetPhoto(int id)
        {
            if (id <= 0) return Json(null);

            var photo = _photoService.GetPhotoById(id);

            return Json(photo != null
                ? $"data:{photo.ImageType};base64,{Convert.ToBase64String(photo.Image)}"
                : null);
        }
        
        [AllowAnonymous]
        public ActionResult PhotoDetails(int id)
        {
            var photo = _photoService.GetPhotoById(id);

            return View("Details", photo);
        }

        [AllowAnonymous]
        public ActionResult AlbumPhotos(int albumId)
        {
            var userId = User.Identity.GetUserId();
            var photos = _photoService.GetPhotosByAlbumId(albumId);

            ViewBag.IsUserOwner = _albumService.IsUserOwner(userId, albumId);
            ViewBag.AlbumId = albumId;

            return View(photos);
        }

        [AllowAnonymous]
        public ActionResult AlbumPhotosByTitle(string title)
        {
            var album = _albumService.GetAlbumByTitle(title);

            if (album == null)
            {
                TempData["Message"] = "Sorry, but there is no album with such direct link";
                return View("AlbumPhotos");
            }

            var userId = User.Identity.GetUserId();
            var photos = _photoService.GetPhotosByAlbumId(album.AlbumId);

            ViewBag.IsUserOwner = _albumService.IsUserOwner(userId, album.AlbumId);
            ViewBag.AlbumId = album.AlbumId;

            return View("AlbumPhotos", photos);
        }

        public ActionResult GetUserPhotos()
        {
            var userId = User.Identity.GetUserId();
            var photos = _photoService.GetPhotosByUserId(userId);
            ViewBag.Albums = _albumService.GetAlbumsByUserId(userId);

            return View("UserPhotos", photos);
        }

        [HttpPost]
        public ActionResult LinkPhotoToAlbum(int photoId, int albumId)
        {
            if (albumId <= 0) return Json("Select album please");
            if (photoId <= 0) return Json("Select photo please");

            _photoService.LinkPhoto(new AlbumPhoto
            {
                PhotoId = photoId,
                AlbumId = albumId
            });

            return Json("ok");
        }

        public ActionResult UnlinkPhoto(int albumId, int photoId)
        {
            if (albumId <= 0 || photoId <= 0)
            {
                TempData["Message"] = "Error, photo wasn't unlinked";
                return RedirectToAction("AlbumPhotos", new { albumId = albumId });
            }

            var userId = User.Identity.GetUserId();
            if(!_photoService.IsUserOwner(userId, photoId) ||
               !_albumService.IsUserOwner(userId, albumId))
            {
                _logger.Info("User ({0}) is trying to unlink photo {1} from album {2}, but he is not the owner", userId, photoId, albumId);
                TempData["Message"] = "Something goes wrong";
                return RedirectToAction("AlbumPhotos", new { albumId = albumId });
            }
            
            _logger.Info("Trying to unlink photo {0} from album {1} by user {2}", photoId, albumId, userId);
            _photoService.UnlinkPhoto(new AlbumPhoto
            {
                AlbumId = albumId,
                PhotoId = photoId
            });
            _logger.Info("Photo {0} was unlinked successfully from album {1} by user {2}", photoId, albumId, userId);

            TempData["Message"] = "Photo was unlinked successfully";
            return RedirectToAction("AlbumPhotos", new { albumId = albumId });
        }

        public ActionResult ChangeAlbumTitlePhoto(int albumId, int photoId)
        {
            if (albumId <= 0 || photoId <= 0)
                return Json("Error, album title wasn't changed");

            var userId = User.Identity.GetUserId();
            if (!_photoService.IsUserOwner(userId, photoId) || !_albumService.IsUserOwner(userId, albumId))
            {
                _logger.Info("User ({0}) is trying to change album ({1}) title to {2}, but he is not the owner", userId, albumId, photoId);
                return Json("Something goes wrong");
            }

            _logger.Info("Album ({0}) title is changing to {1} by {2}", albumId, photoId, userId);
            _photoService.ChangeAlbumTitlePhoto(albumId, photoId);
            _logger.Info("Album ({0}) title was changed to {1} by {2}", albumId, photoId, userId);

            return Json("ok");
        }
        
        #region Add/Edit/Delete

        public ActionResult Edit(int id)
        {
            var photo = _photoService.GetPhotoById(id);

            if (photo == null)
                TempData["Message"] = "You don't have such photo, but you can create new one";

            return View("Edit", Mapper.Map<PhotoModel>(photo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PhotoModel model)
        {
            if (!ModelState.IsValid) return Json("Something went wrong");

            if (!User.CanUserAddPhotos(_photoService))
                return Json("You've headed for the limit of photos (30 photos). For adding more photos you should become a premium user");

            if (!HttpContext.TryGetImageFromRequest(model)) return Json("Image is required");

            var result = model.ValidateImage();
            if (!string.IsNullOrEmpty(result))
                return Json(result);

            model.UserId = User.Identity.GetUserId();
            _logger.Info("Trying to add new photo by user {0}", model.UserId);
            _photoService.Insert(Mapper.Map<Photo>(model));
            _logger.Info("Added new photo by user {0}", model.UserId);

            return Json("ok");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhotoModel model, HttpPostedFileBase imageToUpload)
        {
            if (!ModelState.IsValid || !model.PhotoId.HasValue)
            {
                ModelState.AddModelError("Image", "Something went wrong");
                return View(model);
            }

            if (imageToUpload == null)
            {
                var existingPhoto = _photoService.GetPhotoById(model.PhotoId.Value);
                model.Image = existingPhoto.Image;
                model.ImageType = existingPhoto.ImageType;
            }
            else
            {
                model.MapImage(imageToUpload);

                var result = model.ValidateImage();
                if (!string.IsNullOrEmpty(result))
                {
                    ModelState.AddModelError("Image", result);
                    return View(model);
                }
            }

            model.UserId = User.Identity.GetUserId();
            _logger.Info("Trying to edit photo {0} by user {1}", model.PhotoId, model.UserId);
            _photoService.Update(Mapper.Map<Photo>(model));
            _logger.Info("Photo {0} was edited successfully by user {1}", model.PhotoId, model.UserId);

            TempData["Message"] = "Photo was edited successfully";
            return RedirectToAction("GetUserPhotos");
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            var userId = User.Identity.GetUserId();
            if (!_photoService.IsUserOwner(userId, id))
            {
                _logger.Info("Trying to delete photo {0} by user {1}, but he is not owner", id, userId);
                return Json("Something went wrong");
            }

            _logger.Info("Trying to delete photo {0} by user {1}", id, userId);
            _photoService.Remove(id);
            _logger.Info("Photo {0} was deleted successfully by {1}", id, userId);
            
            return Json("ok");
        }

        #endregion
    }
}