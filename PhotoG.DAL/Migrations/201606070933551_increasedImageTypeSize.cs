namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class increasedImageTypeSize : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Photos", "ImageType");
            AddColumn("dbo.Photos", "ImageType", c => c.String(maxLength:20));
        }
        
        public override void Down()
        {
        }
    }
}
