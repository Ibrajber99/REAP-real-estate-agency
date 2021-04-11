namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingListingImageModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListingImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        ContentType = c.String(nullable: false),
                        content = c.Binary(nullable: false),
                        Listing_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Listings", t => t.Listing_ID)
                .Index(t => t.Listing_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListingImages", "Listing_ID", "dbo.Listings");
            DropIndex("dbo.ListingImages", new[] { "Listing_ID" });
            DropTable("dbo.ListingImages");
        }
    }
}
