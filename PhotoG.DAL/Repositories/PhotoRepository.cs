using System.Data;
using System.Data.Entity;
using System.Linq;
using Dapper;
using PhotoG.DAL.Entities;

namespace PhotoG.DAL.Repositories
{
    public interface IPhotoRepository
    {
        Photo GetPhotoById(int id);
        Photo GetTitlePhoto(int albumId);
        int[] GetPhotosByAlbumId(int id);
        int[] GetPhotosByUserId(string userId);
        void LinkPhoto(AlbumPhoto ap);
        void UnlinkPhoto(AlbumPhoto ap);
        bool IsUserOwner(string userId, int photoId);
        void ChangeAlbumTitlePhoto(int albumId, int photoId);
        int[] Search(string title);
        int[] AdvancedSearch(AdvancedSearchModel photo);
        void Insert(Photo photo);
        void Update(Photo photo);
        void Remove(Photo photo);
    }

    public class PhotoRepository : IPhotoRepository
    {
        public Photo GetPhotoById(int id)
        {
            using (var context = new PhotoGDbContext())
            {
                return context.Photos.AsNoTracking().FirstOrDefault(x => x.PhotoId == id);
            }
        }

        public Photo GetTitlePhoto(int albumId)
        {
            using (var context = new PhotoGDbContext())
            {
                var query = (from a in context.Albums
                             join ap in context.AlbumPhotos on a.AlbumId equals ap.AlbumId
                             join p in context.Photos on ap.PhotoId equals p.PhotoId
                             where a.AlbumId == albumId && ap.IsTitle
                             select p);

                return query.FirstOrDefault();
            }
        }

        public int[] GetPhotosByUserId(string userId)
        {
            using (var context = new PhotoGDbContext())
            {
                return context.Photos
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Select(x => x.PhotoId)
                    .ToArray();
            }
        }

        public int[] GetPhotosByAlbumId(int id)
        {
            using (var context = new PhotoGDbContext())
            {
                var query =(from a in context.Albums
                             join ap in context.AlbumPhotos on a.AlbumId equals ap.AlbumId
                             join p in context.Photos on ap.PhotoId equals p.PhotoId
                             where a.AlbumId == id
                             select p.PhotoId);

                return query.ToArray();
            }
        }

        public void LinkPhoto(AlbumPhoto ap)
        {
            using (var context = new PhotoGDbContext())
            {
                context.Entry(ap).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void UnlinkPhoto(AlbumPhoto ap)
        {
            using (var context = new PhotoGDbContext())
            {
                var albumPhoto = context.AlbumPhotos.FirstOrDefault(x => (x.AlbumId == ap.AlbumId &&
                                                                          x.PhotoId == ap.PhotoId));
                if (albumPhoto == null) return;

                context.AlbumPhotos.Remove(albumPhoto);
                context.SaveChanges();
            }
        }

        public bool IsUserOwner(string userId, int photoId)
        {
            using (var context = new PhotoGDbContext())
            {
                return context.Photos.Any(x => (x.UserId == userId && x.PhotoId == photoId));
            }
        }

        public void ChangeAlbumTitlePhoto(int albumId, int photoId)
        {
            using (var context = new PhotoGDbContext())
            {
                var albumPhoto = context.AlbumPhotos.FirstOrDefault(x => (x.AlbumId == albumId &&
                                                                          x.PhotoId == photoId));
                if (albumPhoto == null) return;

                albumPhoto.IsTitle = true;
                context.SaveChanges();
            }
        }

        public int[] Search(string title)
        {
            using (var ctx = new PhotoGDbContext())
            {
                return ctx.Database.Connection.Query<int>(
                    "spSearchPhotos",
                    new {Title = title},
                    commandType: CommandType.StoredProcedure).ToArray();
            }
        }

        public int[] AdvancedSearch(AdvancedSearchModel model)
        {
            using (var context = new PhotoGDbContext())
            {
                var result = context.Photos
                    .AsQueryable()
                    .PopulateConditions(model)
                    .Select(p => p.PhotoId)
                    .ToArray();

                return result;
            }
        }
        
        public void Insert(Photo photo)
        {
            using (var context = new PhotoGDbContext())
            {
                context.Entry(photo).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Update(Photo photo)
        {
            using (var context = new PhotoGDbContext())
            {
                context.Entry(photo).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Remove(Photo photo)
        {
            using (var context = new PhotoGDbContext())
            {
                var albumPhotos = context.AlbumPhotos.Where(x => x.PhotoId == photo.PhotoId);

                if(albumPhotos.Any())
                    context.AlbumPhotos.RemoveRange(albumPhotos);

                context.Entry(photo).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
