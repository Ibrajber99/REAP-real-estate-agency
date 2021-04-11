namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamingIsArchiveColumnToIsActiveColumnCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Customers", "IsArchived");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "IsArchived", c => c.Boolean(nullable: false));
            DropColumn("dbo.Customers", "IsActive");
        }
    }
}
