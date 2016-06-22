namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AlbumId = c.Int(nullable: false),
                        PhotoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.PhotoId);
            
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.AlbumId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 256),
                        PhotosetDate = c.DateTime(nullable: false),
                        CameraModel = c.String(),
                        LensFocalLength = c.Double(nullable: false),
                        Diaphragm = c.String(),
                        ShutterSpeed = c.Double(nullable: false),
                        ISO = c.Double(nullable: false),
                        FlashInUse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AlbumPhotoes", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.AlbumPhotoes", "AlbumId", "dbo.Albums");
            DropIndex("dbo.AlbumPhotoes", new[] { "PhotoId" });
            DropIndex("dbo.AlbumPhotoes", new[] { "AlbumId" });
            DropTable("dbo.Photos");
            DropTable("dbo.Albums");
            DropTable("dbo.AlbumPhotoes");
        }
    }
}
