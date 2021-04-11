namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatePropToCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "DateUpdated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "DateUpdated");
            DropColumn("dbo.Customers", "DateCreated");
        }
    }
}
