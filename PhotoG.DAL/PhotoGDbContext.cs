
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using PhotoG.DAL.Entities;
using PhotoG.Infrastructure.Logging;

namespace PhotoG.DAL
{
    public class PhotoGDbContext: DbContext
    {
        public PhotoGDbContext() : base("name=PhotoG")
        {
            DatabaseLogger.InitFor(this);
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<AlbumPhoto> AlbumPhotos { get; set; }

        public virtual ObjectResult<int> SearchPhoto(string title)
        {
            var nameParameter = new ObjectParameter("Title", title);

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<int>("spSearchPhoto", nameParameter);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Album

            modelBuilder.Entity<Album>()
                .HasKey(o => o.AlbumId);

            modelBuilder.Entity<Album>()
                .Property(o => o.AlbumId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Album>()
                .Property(o => o.Title)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<Album>()
                .Property(o => o.Description)
                .HasMaxLength(256);

            modelBuilder.Entity<Album>()
                .Property(o => o.UserId)
                .HasMaxLength(128);

            #endregion

            #region Photo

            modelBuilder.Entity<Photo>()
                .HasKey(o => o.PhotoId);

            modelBuilder.Entity<Photo>()
                .Property(o => o.PhotoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Photo>()
                .Property(o => o.Title)
                .HasMaxLength(256);

            modelBuilder.Entity<Photo>()
                .Property(o => o.CameraModel)
                .HasMaxLength(128);

            modelBuilder.Entity<Photo>()
                .Property(o => o.Diaphragm)
                .HasMaxLength(128);

            modelBuilder.Entity<Photo>()
                .Property(o => o.UserId)
                .HasMaxLength(128);

            modelBuilder.Entity<Photo>()
                .Property(o => o.ImageType)
                .HasMaxLength(20);

            #endregion

            #region AlbumPhoto

            modelBuilder.Entity<AlbumPhoto>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<AlbumPhoto>()
                .Property(o => o.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            #endregion
        }
    }
}
