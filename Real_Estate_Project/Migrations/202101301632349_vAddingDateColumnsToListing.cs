namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vAddingDateColumnsToListing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listings", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Listings", "DateUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listings", "DateUpdated");
            DropColumn("dbo.Listings", "DateCreated");
        }
    }
}
