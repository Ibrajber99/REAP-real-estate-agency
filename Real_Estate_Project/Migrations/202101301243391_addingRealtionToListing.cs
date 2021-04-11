namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingRealtionToListing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers");
            DropIndex("dbo.Listings", new[] { "AssociatedAgentID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Listings", "AssociatedAgentID");
            AddForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers", "ID", cascadeDelete: true);
        }
    }
}
