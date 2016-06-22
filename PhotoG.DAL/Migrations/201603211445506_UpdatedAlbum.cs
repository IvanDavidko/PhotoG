namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlbumPhotoes", "IsTitle", c => c.Boolean(nullable: false));
            AddColumn("dbo.Albums", "DirectUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "DirectUrl");
            DropColumn("dbo.AlbumPhotoes", "IsTitle");
        }
    }
}
