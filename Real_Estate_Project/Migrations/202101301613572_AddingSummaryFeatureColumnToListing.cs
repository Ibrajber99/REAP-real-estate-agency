namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSummaryFeatureColumnToListing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listings", "SummaryFeature", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listings", "SummaryFeature");
        }
    }
}
