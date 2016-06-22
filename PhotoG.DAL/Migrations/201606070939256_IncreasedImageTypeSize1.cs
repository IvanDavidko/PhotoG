namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncreasedImageTypeSize1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Photos", "ImageType", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Photos", "ImageType", c => c.String(maxLength: 8));
        }
    }
}
