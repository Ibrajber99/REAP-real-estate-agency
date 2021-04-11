namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingRelationsToCustomer : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Customers", "UserCreatorId");
            CreateIndex("dbo.Customers", "UserUpdatorId");
            AddForeignKey("dbo.Customers", "UserCreatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Customers", "UserUpdatorId", "dbo.OperatingUsers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Customers", "UserCreatorId", "dbo.OperatingUsers");
            DropIndex("dbo.Customers", new[] { "UserUpdatorId" });
            DropIndex("dbo.Customers", new[] { "UserCreatorId" });
        }
    }
}
