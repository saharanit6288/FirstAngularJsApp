namespace DemoWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate_ver010 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HomeBanners", "BannerType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HomeBanners", "BannerType");
        }
    }
}
