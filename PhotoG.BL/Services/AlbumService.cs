using System.Collections.Generic;
using PhotoG.DAL.Entities;
using PhotoG.DAL.Repositories;

namespace PhotoG.BL.Services
{
    public interface IAlbumService
    {
        IEnumerable<Album> GetAlbums();
        Album GetAlbumById(int id);
        IEnumerable<Album> GetAlbumsByUserId(string userId);
        Album GetAlbumByTitle(string title);
        void InsertOrUpdate(Album album);
        void Remove(int id);
        bool IsUserOwner(string userId, int albumId);
    }

    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public IEnumerable<Album> GetAlbums()
        {
            return _albumRepository.GetAlbums();
        }

        public Album GetAlbumById(int id)
        {
            return _albumRepository.GetAlbumById(id);
        }

        public IEnumerable<Album> GetAlbumsByUserId(string userId)
        {
            return _albumRepository.GetAlbumsByUserId(userId);
        }

        public Album GetAlbumByTitle(string title)
        {
            return _albumRepository.GetAlbumByTitle(title);
        }

        public void InsertOrUpdate(Album album)
        {
            _albumRepository.InsertOrUpdate(album);
        }

        public void Remove(int id)
        {
            _albumRepository.Remove(new Album() {AlbumId = id});
        }

        public bool IsUserOwner(string userId, int albumId)
        {
            return _albumRepository.IsUserOwner(userId, albumId);
        }
    }
}
