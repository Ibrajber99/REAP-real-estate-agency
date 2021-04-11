namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingKeysUser : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OperatingUsers", "UserCreatorId");
            CreateIndex("dbo.OperatingUsers", "UserUpdatorId");
            AddForeignKey("dbo.OperatingUsers", "UserCreatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.OperatingUsers", "UserUpdatorId", "dbo.OperatingUsers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatingUsers", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.OperatingUsers", "UserCreatorId", "dbo.OperatingUsers");
            DropIndex("dbo.OperatingUsers", new[] { "UserUpdatorId" });
            DropIndex("dbo.OperatingUsers", new[] { "UserCreatorId" });
        }
    }
}
