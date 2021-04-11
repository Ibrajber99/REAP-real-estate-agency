namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReAddingRelationshipsToListing : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Listings", "AssociatedAgentID");
            CreateIndex("dbo.Listings", "UserCreatorId");
            CreateIndex("dbo.Listings", "UserUpdatorId");
            AddForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers");
            DropIndex("dbo.Listings", new[] { "UserUpdatorId" });
            DropIndex("dbo.Listings", new[] { "UserCreatorId" });
            DropIndex("dbo.Listings", new[] { "AssociatedAgentID" });
        }
    }
}
