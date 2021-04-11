namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingChangesToDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Listings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        AssociatedAgentID = c.Int(nullable: false),
                        ListingAddressID = c.Int(nullable: false),
                        SquareFootage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumOfBeds = c.Int(nullable: false),
                        NumOfBaths = c.Int(nullable: false),
                        CityArea = c.String(nullable: false),
                        UserCreatorId = c.Int(),
                        UserUpdatorId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OperatingUsers", t => t.AssociatedAgentID, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.ListingAddresses", t => t.ListingAddressID, cascadeDelete: true)
                .ForeignKey("dbo.OperatingUsers", t => t.UserCreatorId)
                .ForeignKey("dbo.OperatingUsers", t => t.UserUpdatorId)
                .Index(t => t.CustomerID)
                .Index(t => t.AssociatedAgentID)
                .Index(t => t.ListingAddressID)
                .Index(t => t.UserCreatorId)
                .Index(t => t.UserUpdatorId);
            
            CreateTable(
                "dbo.PropertyFeatures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FeatureName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Heatings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        HeatingType = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ListingAddresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StreetAddress = c.String(nullable: false),
                        Municipality = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        AddressDetails = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PropertyFeaturesListings",
                c => new
                    {
                        PropertyFeatures_ID = c.Int(nullable: false),
                        Listing_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PropertyFeatures_ID, t.Listing_ID })
                .ForeignKey("dbo.PropertyFeatures", t => t.PropertyFeatures_ID, cascadeDelete: true)
                .ForeignKey("dbo.Listings", t => t.Listing_ID, cascadeDelete: true)
                .Index(t => t.PropertyFeatures_ID)
                .Index(t => t.Listing_ID);
            
            CreateTable(
                "dbo.HeatingListings",
                c => new
                    {
                        Heating_ID = c.Int(nullable: false),
                        Listing_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Heating_ID, t.Listing_ID })
                .ForeignKey("dbo.Heatings", t => t.Heating_ID, cascadeDelete: true)
                .ForeignKey("dbo.Listings", t => t.Listing_ID, cascadeDelete: true)
                .Index(t => t.Heating_ID)
                .Index(t => t.Listing_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Listings", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Listings", "UserCreatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Listings", "ListingAddressID", "dbo.ListingAddresses");
            DropForeignKey("dbo.HeatingListings", "Listing_ID", "dbo.Listings");
            DropForeignKey("dbo.HeatingListings", "Heating_ID", "dbo.Heatings");
            DropForeignKey("dbo.PropertyFeaturesListings", "Listing_ID", "dbo.Listings");
            DropForeignKey("dbo.PropertyFeaturesListings", "PropertyFeatures_ID", "dbo.PropertyFeatures");
            DropForeignKey("dbo.Listings", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Listings", "AssociatedAgentID", "dbo.OperatingUsers");
            DropIndex("dbo.HeatingListings", new[] { "Listing_ID" });
            DropIndex("dbo.HeatingListings", new[] { "Heating_ID" });
            DropIndex("dbo.PropertyFeaturesListings", new[] { "Listing_ID" });
            DropIndex("dbo.PropertyFeaturesListings", new[] { "PropertyFeatures_ID" });
            DropIndex("dbo.Listings", new[] { "UserUpdatorId" });
            DropIndex("dbo.Listings", new[] { "UserCreatorId" });
            DropIndex("dbo.Listings", new[] { "ListingAddressID" });
            DropIndex("dbo.Listings", new[] { "AssociatedAgentID" });
            DropIndex("dbo.Listings", new[] { "CustomerID" });
            DropTable("dbo.HeatingListings");
            DropTable("dbo.PropertyFeaturesListings");
            DropTable("dbo.ListingAddresses");
            DropTable("dbo.Heatings");
            DropTable("dbo.PropertyFeatures");
            DropTable("dbo.Listings");
        }
    }
}
