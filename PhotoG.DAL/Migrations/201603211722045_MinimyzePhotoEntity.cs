namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MinimyzePhotoEntity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Photos", "CameraModel", c => c.String(maxLength: 128));
            AlterColumn("dbo.Photos", "Diaphragm", c => c.String(maxLength: 128));
            AlterColumn("dbo.Photos", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Photos", "ImageType", c => c.String(maxLength: 8));
            DropColumn("dbo.Photos", "ImageSize");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "ImageSize", c => c.String());
            AlterColumn("dbo.Photos", "ImageType", c => c.String());
            AlterColumn("dbo.Photos", "UserId", c => c.String());
            AlterColumn("dbo.Photos", "Diaphragm", c => c.String());
            AlterColumn("dbo.Photos", "CameraModel", c => c.String());
        }
    }
}
