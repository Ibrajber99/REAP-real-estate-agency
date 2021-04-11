namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingDriverLicensePropTOUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperatingUserImages", "OperatingUser_ID1", c => c.Int());
            CreateIndex("dbo.OperatingUserImages", "OperatingUser_ID1");
            AddForeignKey("dbo.OperatingUserImages", "OperatingUser_ID1", "dbo.OperatingUsers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatingUserImages", "OperatingUser_ID1", "dbo.OperatingUsers");
            DropIndex("dbo.OperatingUserImages", new[] { "OperatingUser_ID1" });
            DropColumn("dbo.OperatingUserImages", "OperatingUser_ID1");
        }
    }
}
