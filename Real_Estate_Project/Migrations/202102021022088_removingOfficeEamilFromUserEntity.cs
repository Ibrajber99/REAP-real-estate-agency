namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removingOfficeEamilFromUserEntity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OperatingUsers", "OfficeEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OperatingUsers", "OfficeEmail", c => c.String(nullable: false));
        }
    }
}
