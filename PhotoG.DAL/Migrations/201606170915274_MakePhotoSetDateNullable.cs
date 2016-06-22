namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePhotoSetDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Photos", "PhotosetDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Photos", "PhotosetDate", c => c.DateTime(nullable: false));
        }
    }
}
