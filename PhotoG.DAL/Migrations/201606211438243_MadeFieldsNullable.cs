namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeFieldsNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Photos", "LensFocalLength", c => c.Double());
            AlterColumn("dbo.Photos", "ShutterSpeed", c => c.Double());
            AlterColumn("dbo.Photos", "ISO", c => c.Double());
            AlterColumn("dbo.Photos", "FlashInUse", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Photos", "FlashInUse", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Photos", "ISO", c => c.Double(nullable: false));
            AlterColumn("dbo.Photos", "ShutterSpeed", c => c.Double(nullable: false));
            AlterColumn("dbo.Photos", "LensFocalLength", c => c.Double(nullable: false));
        }
    }
}
