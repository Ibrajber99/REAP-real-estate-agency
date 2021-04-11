namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChanginFKRealtionInListingCustomer1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PropertyFeaturesListings", "PropertyFeatures_ID", "dbo.PropertyFeatures");
            DropForeignKey("dbo.PropertyFeaturesListings", "Listing_ID", "dbo.Listings");
            DropForeignKey("dbo.HeatingListings", "Heating_ID", "dbo.Heatings");
            DropForeignKey("dbo.HeatingListings", "Listing_ID", "dbo.Listings");
            DropForeignKey("dbo.Listings", "ListingAddressID", "dbo.ListingAddresses");
            DropForeignKey("dbo.Listings", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Listings", new[] { "CustomerID" });
            DropIndex("dbo.Listings", new[] { "ListingAddressID" });
            DropIndex("dbo.PropertyFeaturesListings", new[] { "PropertyFeatures_ID" });
            DropIndex("dbo.PropertyFeaturesListings", new[] { "Listing_ID" });
            DropIndex("dbo.HeatingListings", new[] { "Heating_ID" });
            DropIndex("dbo.HeatingListings", new[] { "Listing_ID" });
            DropTable("dbo.Listings");
            DropTable("dbo.PropertyFeatures");
            DropTable("dbo.Heatings");
            DropTable("dbo.ListingAddresses");
            DropTable("dbo.PropertyFeaturesListings");
            DropTable("dbo.HeatingListings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HeatingListings",
                c => new
                    {
                        Heating_ID = c.Int(nullable: false),
                        Listing_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Heating_ID, t.Listing_ID });
            
            CreateTable(
                "dbo.PropertyFeaturesListings",
                c => new
                    {
                        PropertyFeatures_ID = c.Int(nullable: false),
                        Listing_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PropertyFeatures_ID, t.Listing_ID });
            
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
                "dbo.Heatings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        HeatingType = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PropertyFeatures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FeatureName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.HeatingListings", "Listing_ID");
            CreateIndex("dbo.HeatingListings", "Heating_ID");
            CreateIndex("dbo.PropertyFeaturesListings", "Listing_ID");
            CreateIndex("dbo.PropertyFeaturesListings", "PropertyFeatures_ID");
            CreateIndex("dbo.Listings", "ListingAddressID");
            CreateIndex("dbo.Listings", "CustomerID");
            AddForeignKey("dbo.Listings", "CustomerID", "dbo.Customers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Listings", "ListingAddressID", "dbo.ListingAddresses", "ID", cascadeDelete: true);
            AddForeignKey("dbo.HeatingListings", "Listing_ID", "dbo.Listings", "ID", cascadeDelete: true);
            AddForeignKey("dbo.HeatingListings", "Heating_ID", "dbo.Heatings", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PropertyFeaturesListings", "Listing_ID", "dbo.Listings", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PropertyFeaturesListings", "PropertyFeatures_ID", "dbo.PropertyFeatures", "ID", cascadeDelete: true);
        }
    }
}
