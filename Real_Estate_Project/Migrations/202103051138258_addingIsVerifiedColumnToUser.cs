namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingIsVerifiedColumnToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperatingUsers", "IsVerified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OperatingUsers", "IsVerified");
        }
    }
}
