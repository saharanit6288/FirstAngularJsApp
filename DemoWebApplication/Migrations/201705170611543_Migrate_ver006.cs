namespace DemoWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate_ver006 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HomeBanners", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HomeBanners", "ImagePath", c => c.String(nullable: false));
        }
    }
}
