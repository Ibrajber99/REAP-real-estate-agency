namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OperatingUsers", "customerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "UserCreatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Customers", "UserUpdatorId", "dbo.OperatingUsers");
            DropIndex("dbo.Customers", new[] { "UserCreatorId" });
            DropIndex("dbo.Customers", new[] { "UserUpdatorId" });
            DropIndex("dbo.OperatingUsers", new[] { "customerId" });
            DropColumn("dbo.OperatingUsers", "customerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OperatingUsers", "customerId", c => c.Int());
            CreateIndex("dbo.OperatingUsers", "customerId");
            CreateIndex("dbo.Customers", "UserUpdatorId");
            CreateIndex("dbo.Customers", "UserCreatorId");
            AddForeignKey("dbo.Customers", "UserUpdatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Customers", "UserCreatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.OperatingUsers", "customerId", "dbo.Customers", "ID");
        }
    }
}
