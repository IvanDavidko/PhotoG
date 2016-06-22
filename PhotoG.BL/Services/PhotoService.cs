using PhotoG.DAL.Entities;
using PhotoG.DAL.Repositories;

namespace PhotoG.BL.Services
{
    public interface IPhotoService
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
        void Remove(int id);
    }

    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;

        public PhotoService(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public Photo GetPhotoById(int id)
        {
            return _photoRepository.GetPhotoById(id);
        }

        public Photo GetTitlePhoto(int albumId)
        {
            return _photoRepository.GetTitlePhoto(albumId);
        }

        public int[] GetPhotosByUserId(string userId)
        {
            return _photoRepository.GetPhotosByUserId(userId);
        }

        public int[] GetPhotosByAlbumId(int id)
        {
            return _photoRepository.GetPhotosByAlbumId(id);
        }

        public void LinkPhoto(AlbumPhoto ap)
        {
            _photoRepository.LinkPhoto(ap);
        }

        public void UnlinkPhoto(AlbumPhoto ap)
        {
            _photoRepository.UnlinkPhoto(ap);
        }

        public bool IsUserOwner(string userId, int photoId)
        {
            return _photoRepository.IsUserOwner(userId, photoId);
        }

        public void ChangeAlbumTitlePhoto(int albumId, int photoId)
        {
            _photoRepository.ChangeAlbumTitlePhoto(albumId, photoId);
        }

        public int[] Search(string title)
        {
            return _photoRepository.Search(title);
        }

        public int[] AdvancedSearch(AdvancedSearchModel photo)
        {
            return _photoRepository.AdvancedSearch(photo);
        }

        public void Insert(Photo photo)
        {
            _photoRepository.Insert(photo);
        }

        public void Update(Photo photo)
        {
            _photoRepository.Update(photo);
        }

        public void Remove(int id)
        {
            _photoRepository.Remove(new Photo() {PhotoId = id});
        }
    }
}
