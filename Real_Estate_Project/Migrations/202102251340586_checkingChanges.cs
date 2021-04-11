namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkingChanges : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PropertyFeaturesListings");
            DropPrimaryKey("dbo.HeatingListings");
            AddPrimaryKey("dbo.PropertyFeaturesListings", new[] { "PropertyFeatures_ID", "Listing_ID" });
            AddPrimaryKey("dbo.HeatingListings", new[] { "Heating_ID", "Listing_ID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.HeatingListings");
            DropPrimaryKey("dbo.PropertyFeaturesListings");
            AddPrimaryKey("dbo.HeatingListings", new[] { "Listing_ID", "Heating_ID" });
            AddPrimaryKey("dbo.PropertyFeaturesListings", new[] { "Listing_ID", "PropertyFeatures_ID" });
        }
    }
}
