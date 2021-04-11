namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChanginFKRealtionInListing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers");
            DropForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers");
            DropIndex("dbo.Listings", new[] { "AssociatedAgentID" });
            DropIndex("dbo.Listings", new[] { "UserCreatorId" });
            DropIndex("dbo.Listings", new[] { "UserUpdatorId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Listings", "UserUpdatorId");
            CreateIndex("dbo.Listings", "UserCreatorId");
            CreateIndex("dbo.Listings", "AssociatedAgentID");
            AddForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers", "ID", cascadeDelete: true);
        }
    }
}
