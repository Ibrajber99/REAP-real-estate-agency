namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checking : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Listings", "OperatingUser_ID", "dbo.OperatingUsers");
            DropIndex("dbo.Listings", new[] { "OperatingUser_ID" });
            DropColumn("dbo.Listings", "OperatingUser_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Listings", "OperatingUser_ID", c => c.Int());
            CreateIndex("dbo.Listings", "OperatingUser_ID");
            AddForeignKey("dbo.Listings", "OperatingUser_ID", "dbo.OperatingUsers", "ID");
        }
    }
}
