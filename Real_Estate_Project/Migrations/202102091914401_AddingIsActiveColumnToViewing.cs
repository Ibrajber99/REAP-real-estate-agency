namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsActiveColumnToViewing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Viewings", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Viewings", "IsActive");
        }
    }
}
