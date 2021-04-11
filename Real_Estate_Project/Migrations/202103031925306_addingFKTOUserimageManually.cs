namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingFKTOUserimageManually : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OperatingUserImages", "OperatingUser_ID", "dbo.OperatingUsers");
            DropIndex("dbo.OperatingUserImages", new[] { "OperatingUser_ID" });
            RenameColumn(table: "dbo.OperatingUserImages", name: "OperatingUser_ID", newName: "ImageRelatedUser");
            AlterColumn("dbo.OperatingUserImages", "ImageRelatedUser", c => c.Int(nullable: false));
            CreateIndex("dbo.OperatingUserImages", "ImageRelatedUser");
            AddForeignKey("dbo.OperatingUserImages", "ImageRelatedUser", "dbo.OperatingUsers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatingUserImages", "ImageRelatedUser", "dbo.OperatingUsers");
            DropIndex("dbo.OperatingUserImages", new[] { "ImageRelatedUser" });
            AlterColumn("dbo.OperatingUserImages", "ImageRelatedUser", c => c.Int());
            RenameColumn(table: "dbo.OperatingUserImages", name: "ImageRelatedUser", newName: "OperatingUser_ID");
            CreateIndex("dbo.OperatingUserImages", "OperatingUser_ID");
            AddForeignKey("dbo.OperatingUserImages", "OperatingUser_ID", "dbo.OperatingUsers", "ID");
        }
    }
}
