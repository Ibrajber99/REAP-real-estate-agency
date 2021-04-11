namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUserIsActiveColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperatingUsers", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OperatingUsers", "IsActive");
        }
    }
}
