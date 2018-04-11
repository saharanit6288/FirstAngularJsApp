namespace DemoWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate_ver009 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "CategoryId");
        }
    }
}
