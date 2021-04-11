namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCUserToCustoemrrelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperatingUsers", "customerId", c => c.Int());
            CreateIndex("dbo.OperatingUsers", "customerId");
            AddForeignKey("dbo.OperatingUsers", "customerId", "dbo.Customers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatingUsers", "customerId", "dbo.Customers");
            DropIndex("dbo.OperatingUsers", new[] { "customerId" });
            DropColumn("dbo.OperatingUsers", "customerId");
        }
    }
}
