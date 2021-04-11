namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPriceColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listings", "Price", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listings", "Price");
        }
    }
}
