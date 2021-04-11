namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingRealtionToListing11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Listings", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Listings", new[] { "CustomerID" });
            RenameColumn(table: "dbo.Listings", name: "CustomerID", newName: "Customer_ID");
            AlterColumn("dbo.Listings", "Customer_ID", c => c.Int());
            CreateIndex("dbo.Listings", "Customer_ID");
            AddForeignKey("dbo.Listings", "Customer_ID", "dbo.Customers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Listings", "Customer_ID", "dbo.Customers");
            DropIndex("dbo.Listings", new[] { "Customer_ID" });
            AlterColumn("dbo.Listings", "Customer_ID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Listings", name: "Customer_ID", newName: "CustomerID");
            CreateIndex("dbo.Listings", "CustomerID");
            AddForeignKey("dbo.Listings", "CustomerID", "dbo.Customers", "ID", cascadeDelete: true);
        }
    }
}
