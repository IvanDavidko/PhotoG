using System;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PhotoG.BL.Services;
using PhotoG.DAL.Entities;
using PhotoG.Infrastructure.Logging;
using PhotoG.UI.Extensions;
using PhotoG.UI.Extentions;
using PhotoG.UI.Models;

namespace PhotoG.UI.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IAlbumService _albumService;
        private readonly ILogger _logger;

        public AlbumController(
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
        public ActionResult GetTitlePhoto(int albumId)
        {
            if (albumId <= 0) return Json(null);

            var photo = _photoService.GetTitlePhoto(albumId);
            
            return Json(photo != null 
                ? $"data:{photo.ImageType};base64,{Convert.ToBase64String(photo.Image)}" 
                : null);
        }

        public ActionResult GetUserAlbums()
        {
            var userId = User.Identity.GetUserId();
            var albums = _albumService.GetAlbumsByUserId(userId);

            return View("UserAlbums", albums);
        }

        #region Add/Edit/Delete
        
        public ActionResult Add()
        {
            var canUserAddAlbums = User.CanUserAddAlbums(_albumService);
            if (!canUserAddAlbums)
                ViewBag.Message = "You've headed for the limit of albums (5 albums). For adding more albums you should become a premium user";

            ViewBag.CanUserAddAlbums = canUserAddAlbums;
            ViewBag.Title = "Add new album";
            return View("AlbumForm");
        }
        
        public ActionResult Edit(int id)
        {
            var album = _albumService.GetAlbumById(id);

            if(album == null)
                ViewBag.Message = "You don't have such album, but you can create new one";

            ViewBag.Title = "Edit album form";
            return View("AlbumForm", Mapper.Map<AlbumModel>(album));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertOrUpdate(AlbumModel model)
        {
            if (!ModelState.IsValid) return View("AlbumForm", model);
            
            if (model.AlbumId == null && !User.CanUserAddAlbums(_albumService))
            {
                ViewBag.Message = "You've headed for the limit of albums (5 albums). For adding more albums you should become a premium user";
                ViewBag.CanUserAddAlbums = false;
                return View("AlbumForm");
            }

            model.CreateUrlFromTitle();
            model.UserId = User.Identity.GetUserId();
            _albumService.InsertOrUpdate(Mapper.Map<Album>(model));

            _logger.Info("Album was added/edited successfully, userId = {0}", model.UserId);

            TempData["Message"] = "Album was added/edited successfully";
            return RedirectToAction("GetUserAlbums");
        }
        
        public ActionResult Remove(int id)
        {
            _albumService.Remove(id);

            _logger.Info("Album {0} was deleted successfully", id);

            TempData["Message"] = "Album was deleted successfully";
            return RedirectToAction("GetUserAlbums");
        }

        #endregion
    }
}