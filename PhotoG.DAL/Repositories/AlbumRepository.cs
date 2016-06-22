
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PhotoG.DAL.Entities;

namespace PhotoG.DAL.Repositories
{
    public interface IAlbumRepository
    {
        IEnumerable<Album> GetAlbums();
        Album GetAlbumById(int id);
        IEnumerable<Album> GetAlbumsByUserId(string userId);
        Album GetAlbumByTitle(string title);
        void InsertOrUpdate(Album album);
        void Remove(Album album);
        bool IsUserOwner(string userId, int albumId);
    }

    public class AlbumRepository: IAlbumRepository
    {
        public IEnumerable<Album> GetAlbums()
        {
            using (var context = new PhotoGDbContext())
            {
                return context.Albums.ToList();
            }
        }
        public Album GetAlbumById(int id)
        {
            using (var context = new PhotoGDbContext())
            {
                return context.Albums.AsNoTracking().FirstOrDefault(a => a.AlbumId == id);
            }
        }
        
        public IEnumerable<Album> GetAlbumsByUserId(string userId)
        {
            using (var context = new PhotoGDbContext())
            {
                return context.Albums.AsNoTracking()
                    .Where(a => a.UserId == userId)
                    .ToList();
            }
        }

        public Album GetAlbumByTitle(string title)
        {
            using (var context = new PhotoGDbContext())
            {
                return context.Albums.AsNoTracking().FirstOrDefault(a => a.Title.Equals(title, StringComparison.Ordinal));
            }
        }

        public bool IsUserOwner(string userId, int albumId)
        {
            using (var context = new PhotoGDbContext())
            {
                return context.Albums.Any(x => (x.UserId == userId && x.AlbumId == albumId));
            }
        }

        public void InsertOrUpdate(Album album)
        {
            using (var context = new PhotoGDbContext())
            {
                context.Entry(album).State = album.AlbumId == 0
                    ? EntityState.Added
                    : EntityState.Modified;

                context.SaveChanges();
            }
        }

        public void Remove(Album album)
        {
            using (var context = new PhotoGDbContext())
            {
                context.AlbumPhotos.RemoveRange(
                    context.AlbumPhotos.Where(x => x.AlbumId == album.AlbumId));

                context.Entry(album).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }
    }
}
