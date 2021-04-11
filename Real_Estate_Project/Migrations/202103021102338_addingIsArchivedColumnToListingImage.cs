namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingIsArchivedColumnToListingImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ListingImages", "IsArchived", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ListingImages", "IsArchived");
        }
    }
}
