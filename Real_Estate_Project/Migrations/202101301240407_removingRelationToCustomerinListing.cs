namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingRelationToCustomerinListing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers");
            DropIndex("dbo.Listings", new[] { "UserCreatorId" });
            DropIndex("dbo.Listings", new[] { "UserUpdatorId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Listings", "UserUpdatorId");
            CreateIndex("dbo.Listings", "UserCreatorId");
            AddForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers", "ID");
        }
    }
}
