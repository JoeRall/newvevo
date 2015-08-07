namespace NewVevo.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreVideoFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "ArtistId", c => c.String());
            AddColumn("dbo.Videos", "VideoYear", c => c.Int(nullable: false));
            AddColumn("dbo.Videos", "TotalViews", c => c.Long(nullable: false));
            AddColumn("dbo.Videos", "Genre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "Genre");
            DropColumn("dbo.Videos", "TotalViews");
            DropColumn("dbo.Videos", "VideoYear");
            DropColumn("dbo.Videos", "ArtistId");
        }
    }
}
