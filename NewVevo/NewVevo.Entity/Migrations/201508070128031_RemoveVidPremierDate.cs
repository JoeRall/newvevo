namespace NewVevo.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveVidPremierDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Videos", "Premiered");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Videos", "Premiered", c => c.DateTime(nullable: false));
        }
    }
}
