namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingUserFKNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.OperatingUsers", new[] { "UserCreatorId" });
            AlterColumn("dbo.OperatingUsers", "UserCreatorId", c => c.Int());
            CreateIndex("dbo.OperatingUsers", "UserCreatorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OperatingUsers", new[] { "UserCreatorId" });
            AlterColumn("dbo.OperatingUsers", "UserCreatorId", c => c.Int(nullable: false));
            CreateIndex("dbo.OperatingUsers", "UserCreatorId");
        }
    }
}
