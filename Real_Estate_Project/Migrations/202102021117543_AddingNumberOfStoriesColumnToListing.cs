namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingNumberOfStoriesColumnToListing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Listings", "NumOfStories", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Listings", "NumOfStories");
        }
    }
}
