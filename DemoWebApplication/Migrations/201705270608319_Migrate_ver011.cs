namespace DemoWebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate_ver011 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Discount");
        }
    }
}
