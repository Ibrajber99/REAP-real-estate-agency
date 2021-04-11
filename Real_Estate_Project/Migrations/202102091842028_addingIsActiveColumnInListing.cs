namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingIsActiveColumnInListing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listings", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listings", "IsActive");
        }
    }
}
