namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePhotoEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "UserId", c => c.String());
            AddColumn("dbo.Photos", "Image", c => c.Binary());
            AddColumn("dbo.Photos", "ImageSize", c => c.String());
            AddColumn("dbo.Photos", "ImageType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "ImageType");
            DropColumn("dbo.Photos", "ImageSize");
            DropColumn("dbo.Photos", "Image");
            DropColumn("dbo.Photos", "UserId");
        }
    }
}
