namespace PhotoG.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "UserId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "UserId");
        }
    }
}
