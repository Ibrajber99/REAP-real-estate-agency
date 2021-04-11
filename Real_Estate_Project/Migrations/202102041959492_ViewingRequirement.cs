namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ViewingRequirement : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers");
            DropForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers");
            DropIndex("dbo.Viewings", new[] { "Customer_ID" });
            DropIndex("dbo.Viewings", new[] { "ViewingHost_ID" });
            AlterColumn("dbo.Viewings", "Customer_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Viewings", "ViewingHost_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Viewings", "Customer_ID");
            CreateIndex("dbo.Viewings", "ViewingHost_ID");
            AddForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers");
            DropForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers");
            DropIndex("dbo.Viewings", new[] { "ViewingHost_ID" });
            DropIndex("dbo.Viewings", new[] { "Customer_ID" });
            AlterColumn("dbo.Viewings", "ViewingHost_ID", c => c.Int());
            AlterColumn("dbo.Viewings", "Customer_ID", c => c.Int());
            CreateIndex("dbo.Viewings", "ViewingHost_ID");
            CreateIndex("dbo.Viewings", "Customer_ID");
            AddForeignKey("dbo.Viewings", "ViewingHost_ID", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Viewings", "Customer_ID", "dbo.Customers", "ID");
        }
    }
}
