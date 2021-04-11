namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletingAllOtherContextes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers");
            DropIndex("dbo.Viewings", new[] { "Listing_ID" });
            DropIndex("dbo.Viewings", new[] { "ViewingHost_ID" });
            DropIndex("dbo.Viewings", new[] { "OperatingUser_ID" });
            DropColumn("dbo.Viewings", "ViewingHost_ID");
            RenameColumn(table: "dbo.Viewings", name: "OperatingUser_ID", newName: "ViewingHost_ID");
            DropPrimaryKey("dbo.PropertyFeaturesListings");
            DropPrimaryKey("dbo.HeatingListings");
            AlterColumn("dbo.Viewings", "Listing_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Viewings", "ViewingHost_ID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PropertyFeaturesListings", new[] { "Listing_ID", "PropertyFeatures_ID" });
            AddPrimaryKey("dbo.HeatingListings", new[] { "Listing_ID", "Heating_ID" });
            CreateIndex("dbo.Viewings", "Listing_ID");
            CreateIndex("dbo.Viewings", "ViewingHost_ID");
            AddForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers", "ID");
            AddForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers");
            DropForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers");
            DropIndex("dbo.Viewings", new[] { "ViewingHost_ID" });
            DropIndex("dbo.Viewings", new[] { "Listing_ID" });
            DropPrimaryKey("dbo.HeatingListings");
            DropPrimaryKey("dbo.PropertyFeaturesListings");
            AlterColumn("dbo.Viewings", "ViewingHost_ID", c => c.Int());
            AlterColumn("dbo.Viewings", "Listing_ID", c => c.Int());
            AddPrimaryKey("dbo.HeatingListings", new[] { "Heating_ID", "Listing_ID" });
            AddPrimaryKey("dbo.PropertyFeaturesListings", new[] { "PropertyFeatures_ID", "Listing_ID" });
            RenameColumn(table: "dbo.Viewings", name: "ViewingHost_ID", newName: "OperatingUser_ID");
            AddColumn("dbo.Viewings", "ViewingHost_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Viewings", "OperatingUser_ID");
            CreateIndex("dbo.Viewings", "ViewingHost_ID");
            CreateIndex("dbo.Viewings", "Listing_ID");
            AddForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers", "ID", cascadeDelete: true);
        }
    }
}
