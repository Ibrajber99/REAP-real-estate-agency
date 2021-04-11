namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingPriceColumnToNotNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Listings", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Listings", "Price", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
