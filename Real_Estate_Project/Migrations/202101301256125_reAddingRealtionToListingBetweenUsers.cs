namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reAddingRealtionToListingBetweenUsers : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Listings", "UserCreatorId");
            CreateIndex("dbo.Listings", "UserUpdatorId");
            AddForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers");
            DropIndex("dbo.Listings", new[] { "UserUpdatorId" });
            DropIndex("dbo.Listings", new[] { "UserCreatorId" });
        }
    }
}
