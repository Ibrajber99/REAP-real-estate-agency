namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialViewing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Viewings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserCreatorId = c.Int(),
                        UserUpdatorId = c.Int(),
                        Customer_ID = c.Int(),
                        Listing_ID = c.Int(),
                        ViewingHost_ID = c.Int(),
                        OperatingUser_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.Customer_ID)
                .ForeignKey("dbo.Listings", t => t.Listing_ID)
                .ForeignKey("dbo.OperatingUsers", t => t.UserCreatorId)
                .ForeignKey("dbo.OperatingUsers", t => t.UserUpdatorId)
                .ForeignKey("dbo.OperatingUsers", t => t.ViewingHost_ID)
                .ForeignKey("dbo.OperatingUsers", t => t.OperatingUser_ID)
                .Index(t => t.UserCreatorId)
                .Index(t => t.UserUpdatorId)
                .Index(t => t.Customer_ID)
                .Index(t => t.Listing_ID)
                .Index(t => t.ViewingHost_ID)
                .Index(t => t.OperatingUser_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Viewings", "OperatingUser_ID", "dbo.OperatingUsers");
            DropForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers");
            DropForeignKey("dbo.Viewings", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Viewings", "UserCreatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Viewings", "Listing_ID", "dbo.Listings");
            DropForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers");
            DropIndex("dbo.Viewings", new[] { "OperatingUser_ID" });
            DropIndex("dbo.Viewings", new[] { "ViewingHost_ID" });
            DropIndex("dbo.Viewings", new[] { "Listing_ID" });
            DropIndex("dbo.Viewings", new[] { "Customer_ID" });
            DropIndex("dbo.Viewings", new[] { "UserUpdatorId" });
            DropIndex("dbo.Viewings", new[] { "UserCreatorId" });
            DropTable("dbo.Viewings");
        }
    }
}
