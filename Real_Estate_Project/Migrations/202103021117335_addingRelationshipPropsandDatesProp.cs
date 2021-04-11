namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingRelationshipPropsandDatesProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ListingImages", "UserCreatorId", c => c.Int());
            AddColumn("dbo.ListingImages", "UserUpdatorId", c => c.Int());
            AddColumn("dbo.ListingImages", "DateAdded", c => c.DateTime(nullable: false));
            AddColumn("dbo.ListingImages", "DateArchived", c => c.DateTime());
            CreateIndex("dbo.ListingImages", "UserCreatorId");
            CreateIndex("dbo.ListingImages", "UserUpdatorId");
            AddForeignKey("dbo.ListingImages", "UserCreatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.ListingImages", "UserUpdatorId", "dbo.OperatingUsers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListingImages", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.ListingImages", "UserCreatorId", "dbo.OperatingUsers");
            DropIndex("dbo.ListingImages", new[] { "UserUpdatorId" });
            DropIndex("dbo.ListingImages", new[] { "UserCreatorId" });
            DropColumn("dbo.ListingImages", "DateArchived");
            DropColumn("dbo.ListingImages", "DateAdded");
            DropColumn("dbo.ListingImages", "UserUpdatorId");
            DropColumn("dbo.ListingImages", "UserCreatorId");
        }
    }
}
