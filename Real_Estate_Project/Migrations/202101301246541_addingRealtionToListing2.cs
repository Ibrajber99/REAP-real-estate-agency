namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingRealtionToListing2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Listings", "Customer_ID", "dbo.Customers");
            DropIndex("dbo.Listings", new[] { "Customer_ID" });
            RenameColumn(table: "dbo.Listings", name: "Customer_ID", newName: "CustomerID");
            AlterColumn("dbo.Listings", "CustomerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Listings", "CustomerID");
            AddForeignKey("dbo.Listings", "CustomerID", "dbo.Customers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Listings", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Listings", new[] { "CustomerID" });
            AlterColumn("dbo.Listings", "CustomerID", c => c.Int());
            RenameColumn(table: "dbo.Listings", name: "CustomerID", newName: "Customer_ID");
            CreateIndex("dbo.Listings", "Customer_ID");
            AddForeignKey("dbo.Listings", "Customer_ID", "dbo.Customers", "ID");
        }
    }
}
