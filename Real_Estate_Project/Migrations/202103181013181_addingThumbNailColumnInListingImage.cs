namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingThumbNailColumnInListingImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ListingImages", "ThumbnailContent", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ListingImages", "ThumbnailContent");
        }
    }
}
