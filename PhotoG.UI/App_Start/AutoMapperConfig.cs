using AutoMapper;
using PhotoG.DAL.Entities;
using PhotoG.UI.Models;

namespace PhotoG.UI
{
    public static class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.CreateMap<AlbumModel, Album>();
            Mapper.CreateMap<Album, AlbumModel>();

            Mapper.CreateMap<PhotoModel, Photo>();
            Mapper.CreateMap<Photo, PhotoModel>()
                .ForMember(x => x.ImageSize, option => option.Ignore());
        }
    }
}