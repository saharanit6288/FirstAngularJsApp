namespace DemoWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate_ver005 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HomeBanners", "Title", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.HomeBanners", "ImagePath", c => c.String(nullable: false));
            CreateIndex("dbo.HomeBanners", "Title", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.HomeBanners", new[] { "Title" });
            AlterColumn("dbo.HomeBanners", "ImagePath", c => c.String());
            AlterColumn("dbo.HomeBanners", "Title", c => c.String());
        }
    }
}
