namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reAddingRealtionToListingBetweenAsosciatedAgent : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Listings", "AssociatedAgentID");
            AddForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers");
            DropIndex("dbo.Listings", new[] { "AssociatedAgentID" });
        }
    }
}
